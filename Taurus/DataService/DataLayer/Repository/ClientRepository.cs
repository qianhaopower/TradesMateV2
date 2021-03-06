﻿using DataService.Infrastructure;
using DataService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace EF.Data
{

    public class ClientRepository : BaseRepository, IClientRepository
    {
        private readonly IAuthRepository _authRepo;
        private readonly IPropertyRepository _propertyRepo;
        private readonly IMessageRepository _messageRepository;
        private readonly ICompanyRepository _companyRepository;

        public ClientRepository(EFDbContext ctx, ApplicationUserManager manager, IAuthRepository authRepo,IPropertyRepository propertyRepo, IMessageRepository messageRepository, ICompanyRepository companyRepository) :base(ctx, manager)
        {
            _authRepo = authRepo;
            _propertyRepo = propertyRepo;
            _messageRepository = messageRepository;
            _companyRepository = companyRepository;
        }
       
        public IQueryable<Client> GetAccessibleClientForUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception("User name cannot by empty");
            }
            var user =  _userManager.FindByName(userName);

            IQueryable<Client> clients = null;  
            if(user != null)
            {
                //if current user is a client, only show property that linked to him/her.
                if(user.UserType == (int)UserType.Client )//check lazy loading
                {
                    //can only see him/herself

                    //throw new Exception(string.Format("{0} cannot view client list", userName));
                    ////load the client
                    _ctx.Entry(user).Reference(s => s.Client).Load();

                    clients = _ctx.Clients.Where(p => p.Id == user.Client.Id);

                }
                else if (user.UserType == UserType.Trade)
                {
                    var properties = _propertyRepo.GetPropertyForUser(userName);

                    clients = GetClientsForProperty(properties);

                    //need get all the client that has a work request. 
                    var clientHasRequest = _messageRepository.GetClientThatHasMessageForUser(userName);
                    clients = clients.Union(clientHasRequest);
                }
                else
                {
                    throw new Exception("Unknown user type");
                }
              
            }
            else
            {
                throw new Exception("Cannot find user." + userName);
            }

            return clients;
        }

        public Client GetClient(string userName, int clientId)
        {
            return GetAccessibleClientForUser(userName).FirstOrDefault(c => c.Id == clientId);
        }
        public Client UpdateClient(string username, ClientModel model)
        {
            if (!GetAccessibleClientForUser(username).Any(p => p.Id == model.Id))
                throw new Exception($"No permission to edit client {model.Id}");
            
            var toEditClient = _ctx.Clients.First(p => p.Id == model.Id);
            if (toEditClient == null) return toEditClient;

            toEditClient.ModifiedDateTime = DateTime.Now;
            toEditClient.Description = model.Description;
            toEditClient.FirstName = model.FirstName;
            toEditClient.LastName = model.LastName;
            toEditClient.MobileNumber = model.MobileNumber;
            // All other properties are linked to the user record. Need change it when changing user.


            _ctx.Entry(toEditClient).State = EntityState.Modified;
            _ctx.SaveChanges();
            return toEditClient;
            
        }

        public void RemoveClient(string userName, int clientId)
        {
            var company = _companyRepository.GetCompanyFoAdminUser(userName);
            var isUserAdmin = _authRepo.isUserAdmin(userName);
            if (!isUserAdmin)
            {
                throw new Exception("Only Admin user can delete client");
            }

            var clientToDelete = GetAccessibleClientForUser(userName).FirstOrDefault(c => c.Id == clientId);
            if(clientToDelete != null)
            {
                //need remove all propertyCompany records for that company, for that client's property
                var propertyIdsForClient = _ctx.ClientProperties.Where(p => p.ClientId == clientId).Select(p=> p.PropertyId);
                //do not delete system property
                var propertyIdsNotSystem = _ctx.Properties
                    .Where(p => propertyIdsForClient.Contains(p.Id) && !p.SystemPropertyCompanyId.HasValue)
                    .Select(p => p.Id).ToList();
                if (propertyIdsNotSystem.Any())
                {
                    _ctx.PropertyCompanies.Where(c => propertyIdsNotSystem.Contains(c.PropertyId) && c.CompanyId == company.Id).Delete();
                }

                //also need remove client and the system property relation
                var systemProperty = _ctx.Properties.FirstOrDefault(p => p.SystemPropertyCompanyId == company.Id);
                if(systemProperty != null)
                _ctx.ClientProperties.Where(c => c.ClientId == clientId && c.PropertyId == systemProperty.Id).Delete();


                _ctx.SaveChanges();
            }
        }

        private IQueryable<Client> GetClientsForProperty(IQueryable<Property> list)
        {
            var clients = from p in list
                          join cp in _ctx.ClientProperties on p.Id equals cp.PropertyId
                          join c in _ctx.Clients on cp.ClientId equals c.Id
                          select c;
            return clients;

        }
        public string BulkCreateClient(string adminUserName, List<CreateNewClientBulkModel> models, ApplicationUserManager appUserManager)
        {
            var company = _companyRepository.GetCompanyFoAdminUser(adminUserName);
            //check if system property for this company existed, if not create
            var systemProperty = _ctx.Properties.FirstOrDefault(p => p.SystemPropertyCompanyId.HasValue && p.SystemPropertyCompanyId.Value == company.Id);
            if (systemProperty == null)
            {

                Property propertySystem = new Property
                {
                    Name = "SystemProperty",
                    Description = $"This is the system property for {company.Name}",
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    SystemPropertyCompanyId = company.Id,
                };
                _ctx.Entry(propertySystem).State = EntityState.Added;
                _ctx.SaveChanges();
                systemProperty = _ctx.Properties.First(p => p.SystemPropertyCompanyId.HasValue && p.SystemPropertyCompanyId.Value == company.Id);
            }

            //add this property to current company
            var systemPropertyCompany =
                _ctx.PropertyCompanies.FirstOrDefault(p =>
                    p.PropertyId == systemProperty.Id && p.CompanyId == company.Id);
            if (systemPropertyCompany == null)
            {
                var newPropertyCompany = new PropertyCompany()
                {
                    CompanyId =  company.Id,
                    PropertyId = systemProperty.Id,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now
                };
                _ctx.PropertyCompanies.Add(newPropertyCompany);
            }
            foreach (var client in models.Where(c=> !string.IsNullOrEmpty(c.FirstName) && !string.IsNullOrEmpty(c.LastName) && !string.IsNullOrEmpty(c.EmailAddress)))
            {
                var singleClientModel = new CreateNewClientRequestModel()
                {
                    Address = new AddressModel()
                    {
                        Line1 = client.HomeStreet,
                        City =  client.HomeCity,
                        State = client.HomeState,
                        PostCode = client.HomePostalCode,
                        Suburb = client.HomePostalCode,
                    },
                    Client = new ClientModel()
                    {
                        MobileNumber = client.HomePhone,
                        Email = client.EmailAddress,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Description = $"{client.Company} {company.Website}"
                    },
                    PropertyId = systemProperty.Id
                };
                CreateClient(adminUserName, singleClientModel, appUserManager);
            }

            _ctx.SaveChanges();

            return "Successfull";

        }

        public string CreateClient(string adminUserName, CreateNewClientRequestModel model, ApplicationUserManager manager)
        {

            if (model.Client == null || !IsValidEmail(model.Client.Email))
            {
                throw new Exception("Client must have a valid email");
            }
            var existingClient = _ctx.Clients.FirstOrDefault(c => c.Email == model.Client.Email);
            if (existingClient != null)
            {
                if (_ctx.ClientProperties.Any(p=> p.PropertyId == model.PropertyId && p.ClientId == existingClient.Id))
                {
                    return "No need to create";
                }
                if (model.PropertyId  == 0)
                {
                    throw new Exception("Must request for a property");
                }

                var propertyClient = new ClientProperty()
                {
                    PropertyId = model.PropertyId,
                    ClientId = existingClient.Id,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now
                };
                _ctx.ClientProperties.Add(propertyClient);
                _ctx.SaveChanges();
                return "Client attached to your company";
            }
            else
            {
                return CreateNewClient(model, manager, adminUserName).Result;
            }
        }

      

        private async Task<string> CreateNewClient(CreateNewClientRequestModel model, ApplicationUserManager manger, string adminUserName)
        {
            if (model.Address == null)
            {
                throw new Exception("New client must have a property");
            }
            if (model.Client.FirstName == null)
            {
                throw new Exception("New client must have a first name");
            }
            if (model.Client.LastName == null)
            {
                throw new Exception("New client must have a last name");
            }

            //if (model.Client.MobileNumber == null)
            //{
            //    throw new Exception("New client must have a mobile number");
            //}

            //1 create client
            var userName = $"{model.Client.FirstName.ToLower()}.{model.Client.LastName.ToLower()}";
            var result = await _authRepo.RegisterUser(new UserModel
            {
                Email = model.Client.Email,
                FirstName = model.Client.FirstName,
                LastName = model.Client.LastName,
                UserName = userName,
                UserType = (int)UserType.Client
            }, manger);
            if (result.Succeeded)
            {
                //2 create property
                var newClient = _authRepo.GetClientByUserName(userName);
                var newProperty = _propertyRepo.CreatePropertyForClient(adminUserName, new PropertyModel
                {
                    Address = model.Address,
                    ClientId = newClient.Id,
                    Name = $"{model.Client.FirstName}'s property"
                });
                return "New client created";
            }
            
                throw new Exception( $"Failed to register client due to {result.Errors.First()}");
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}