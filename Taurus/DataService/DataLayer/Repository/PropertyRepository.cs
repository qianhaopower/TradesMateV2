﻿
using AutoMapper;
using DataService.Entities;
using DataService.Infrastructure;
using DataService.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace EF.Data
{

    public class PropertyRepository : BaseRepository, IPropertyRepository
    {
        private readonly IAuthRepository _authRepository;
        private readonly ICompanyRepository _companyRepository;
        public PropertyRepository(EFDbContext ctx, ApplicationUserManager manager, IAuthRepository authRepo, ICompanyRepository companyRepository) : base(ctx, manager)
        {
            
            _companyRepository = companyRepository;
            _authRepository = authRepo;
        }

        public Property CreatePropertyForClient(string userName, PropertyModel model)
        {
            var companyId = _companyRepository.GetCompanyFoAdminUser(userName).Id;
            if (model.Address == null)
            {
                throw new Exception("Address cannot be null");
            }

            Property newProperty = new Property
            {
                Name = model.Name,
                Address = new Address()
                {
                    City = model.Address.City,
                    Line1 = model.Address.Line1,
                    Line2 = model.Address.Line2,
                    Line3 = model.Address.Line3,
                    PostCode = model.Address.PostCode,
                    State = model.Address.State,
                    Suburb = model.Address.Suburb,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                },
                Description = model.Description,
                Condition = model.Condition,
                Narrative = model.Narrative,
                Comment = model.Comment,
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,

            };
            _ctx.Entry(newProperty).State = EntityState.Added;

            //add this property to the company
            PropertyCompany propertyCompanyNew = new PropertyCompany
            {
                Property = newProperty,
                CompanyId = companyId,
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
            };
            _ctx.Entry(propertyCompanyNew).State = EntityState.Added;



            ClientProperty clientPropertyNew = new ClientProperty
            {
                ClientId = model.ClientId,
                Property = newProperty,
                Confirmed = true,
                Role = ClientRole.Owner,
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
            };

            CreateDefaultSections(model.DefaultSections, newProperty, _ctx);

            _ctx.Entry(clientPropertyNew).State = EntityState.Added;
            _ctx.SaveChanges();
            return newProperty;
        }

        public Property UpdatePropertyForClient(string username, PropertyModel model)
        {
            if (!GetPropertyForUser(username).Any(p => p.Id == model.Id))
                throw new Exception($"No permission to edit client {model.Id}");

            var toEditProperty = _ctx.Properties.Include(a => a.Address).First(p => p.Id == model.Id);
            if (toEditProperty == null) return null;

            toEditProperty.ModifiedDateTime = DateTime.Now;
            toEditProperty.Description = model.Description;
            toEditProperty.Comment = model.Comment;
            toEditProperty.Condition = model.Condition;
            toEditProperty.Name = model.Name;
            toEditProperty.Narrative = model.Narrative;
            if(model.Address != null)
            {
                toEditProperty.Address.ModifiedDateTime = DateTime.Now;
                toEditProperty.Address.Line1 = model.Address.Line1;
                toEditProperty.Address.Line2 = model.Address.Line2;
                toEditProperty.Address.Line3 = model.Address.Line3;
                toEditProperty.Address.PostCode = model.Address.PostCode;
                toEditProperty.Address.State = model.Address.State;
                toEditProperty.Address.City = model.Address.City;
            }
            toEditProperty.Address.ModifiedDateTime = DateTime.Now;
            toEditProperty.Address.Line1 = model.Address.Line1;
            _ctx.Entry(toEditProperty).State = EntityState.Modified;
            _ctx.SaveChanges();
            return toEditProperty;
        }

        public void DeleteProperty(string username, int key)
        {
            var isUserAdmin = _authRepository.isUserAdmin(username);
            if (!isUserAdmin)
            {
                throw new Exception("Only Admin user can delete property");
            }
            Property property = _ctx.Properties.Find(key);
            if (property == null)
            {
                throw new Exception($"Property with Id {key} cannot be found.");
            }

            _ctx.Properties.Remove(property);
            _ctx.ClientProperties.RemoveRange(_ctx.ClientProperties.Where(p => p.PropertyId == key).ToList());
            _ctx.PropertyCompanies.RemoveRange(_ctx.PropertyCompanies.Where(p => p.PropertyId == key).ToList());
            _ctx.PropertyAllocations.RemoveRange(_ctx.PropertyAllocations.Where(p => p.PropertyId == key).ToList());

            _ctx.Attchments.RemoveRange(_ctx.Attchments.Where(p => p.EntityId == key && p.EntityType == AttachmentEntityType.Property).ToList());


            var messagesForProperty = _ctx.Messages.Where(p => p.PropertyId == key).ToList();
            var messagesResponseForProperty = _ctx.Messages.Where(p => p.PropertyId == key && p.MessageResponse != null).Select(p => p.MessageResponse).ToList();
            //var messagesResponseForProperty = _ctx.MessageResponses.Where(p => p.Message != null && messagesForProperty.Select(w=> w.Id).Contains(p.Message.Id)).ToList();

            _ctx.Messages.RemoveRange(messagesForProperty);
            _ctx.MessageResponses.RemoveRange(messagesResponseForProperty);

            var sections = _ctx.Sections.Where(p => p.PropertyId == key).Include(p => p.WorkItemList).ToList();
            var workItemForSection = sections.SelectMany(p => p.WorkItemList).ToList();

            _ctx.WorkItems.RemoveRange(workItemForSection);
            _ctx.Sections.RemoveRange(sections);


            _ctx.SaveChanges();
        }

        public void CreateDefaultSections(DefaultPropertySection model, Property parentProperty, EFDbContext context)
        {

            Func<int, SectionType, List<Section>> createFunc = (int number, SectionType type) => {
                List<Section> result = new List<Section>();
                for (int k = 0; k < number; k++)
                {
                    var name = GetSectionNameFromEnum(type);
                    Section sec = new Section
                    {
                        Name = name + " " + (k+1),//start from bedroom 1
                        Description = name + " " + (k + 1),
                        Property = parentProperty,
                        Type = name,
                        AddedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                    };
                    result.Add(sec);
                    context.Entry(sec).State = EntityState.Added;
                };
                return result;
            };

            if (model != null)
            {
                createFunc(model.BasementNumber, SectionType.Basement);
                createFunc(model.BathroomNumber, SectionType.Bathroom);
                createFunc(model.BedroomNumber, SectionType.Bedroom);
                createFunc(model.DeckNumber, SectionType.Deck);
                createFunc(model.GarageNumber, SectionType.Garage);
                createFunc(model.GardenNumber, SectionType.Garden);
                createFunc(model.HallWayNumber, SectionType.HallWay);
                createFunc(model.KitchenNumber, SectionType.Kitchen);
                createFunc(model.LaundryRoomNumber, SectionType.LaundryRoom);
                createFunc(model.LivingRoomNumber, SectionType.LivingRoom);
            }

        }

        public string GetSectionNameFromEnum(SectionType type)
        {
            switch (type) {
                case SectionType.Basement:
                    return "Basement";
                case SectionType.Bathroom:
                    return "Bathroom";
                case SectionType.Bedroom:
                    return "Bedroom";
                case SectionType.Deck:
                    return "Deck";
                case SectionType.Garage:
                    return "Garage";
                case SectionType.Garden:
                    return "Garden";
                case SectionType.HallWay:
                    return "Hallway";
                case SectionType.Kitchen:
                    return "Kitchen";
                case SectionType.LaundryRoom:
                    return "Laundry room";
                case SectionType.LivingRoom:
                    return "Living room";
                default:
                    throw new Exception("Invalid section type");
            }

        }

        public IQueryable<Property> GetPropertyForUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception("User name cannot by empty");
            }
            var user =  _userManager.FindByName(userName);

            IQueryable<Property> properties = null;  
            if(user != null)
            {
                //if current user is a client, only show property that linked to him/her.
                if(user.UserType == (int)UserType.Client )//check lazy loading
                {
      
                    //load the client
                    _ctx.Entry(user).Reference(s => s.Client).Load();

                    properties = GetClientProperties(user.Client.Id);

                }
                else if (user.UserType == UserType.Trade)
                {
                    _ctx.Entry(user).Reference(s => s.Member).Load();

                    properties = GetMemberProperties(user.Member.Id);

                }
                else
                {
                    throw new Exception("Unknown user type");
                }

                //if curreny user is a tradesman, only show property for his/her company. 
            }
            else
            {
                throw new Exception("Cannot find user." + userName);
            }

            return properties;
           

        }

        public List<Section> GetPropertySectionList(string name, int key)
        {
            var sections = this.GetPropertyForUser(name)
                .Where(p => p.Id == key)
                .Include(p=> p.SectionList)
                .SelectMany(m => m.SectionList)
                .ToList();
            return sections; 
        }

        public async Task<PropertyReport> GetPropertyReportData(int propertyId, string userName)
        {
            var isContractor = await _authRepository.IsUserContractorAsync(userName);
            var hasPermission = GetPropertyForUser(userName).Any(p => p.Id == propertyId);
            if (!hasPermission || isContractor)
                throw new Exception("No permission to get report for property with id " + propertyId);

            var groups = GetReportGroups(propertyId,userName);
            var propertyInfo = this.GetProperty(userName, propertyId);
            propertyInfo.PropertyLogoUrl = this.GetPropertyLogoUrl(propertyId);

            var company = _companyRepository.GetCompanyForUser(userName);
            var companyModel = Mapper.Map<Company, CompanyModel>(company);
            companyModel.CompanyLogoUrl = _companyRepository.GetCompanyLogoUrl(company.Id);
            return new PropertyReport()
            {
                CompanyInfo = companyModel,
                PropertyInfo = propertyInfo,
                ReportGroupitem = groups
            };
        }
        private string GetPropertyLogoUrl(int propertyId)
        {
            var logo = _ctx.Attchments.OrderByDescending(p => p.AddedDateTime)
                .FirstOrDefault(p => p.EntityType == AttachmentEntityType.Property && p.EntityId == propertyId);
            return logo?.Url;
        }


        public PropertyModel GetProperty(string userName, int propertyId)
        {
            var property = GetPropertyForUser(userName).Include(p => p.Address).FirstOrDefault(p => p.Id == propertyId);
             return Mapper.Map<Property, PropertyModel>(property);
        }


        private List<PropertyReportGroupItem> GetReportGroups(int propertyId, string userName)
        {
            var result = new List<PropertyReportGroupItem>();
            var rawData = _ctx.Properties.Include(p => p.SectionList.Select(z => z.WorkItemList)).Single(p => p.Id == propertyId);
            rawData.SectionList.Where(s => s.WorkItemList.Any()).ToList().ForEach(p => result.Add(new PropertyReportGroupItem()
            {
                SectionName = p.Name,
                workItems = p.WorkItemList.Select(x => new WorkItemModel()
                {
                    Description = x.Description,
                    Name = x.Name,
                    Quantity = x.Quantity,
                    Status = x.Status.ToString(),
                    TaskNumber = 0,
                    TradeWorkType = x.TradeWorkType,
                    Id = x.Id,
                    WorkItemTemplateId = x.WorkItemTemplateId

                }).ToList(),
            }));

            //result.ForEach(p => p.workItems.ForEach(x => x.TaskNumber = i++));

            //get attachments URL
            //set up task number
            int i = 1;
            var images = GetPropertyWorkItemsAttachments(propertyId, userName).Where(p => p.Type == AttachmentType.Image).ToList();

            var workItemForProperty = GetAllPropertyWorkItems(propertyId).ToList();
            var allWorkItemTemplateIds = workItemForProperty.Select(w => w.WorkItemTemplateId);
            var allTemplateImages = _ctx.Attchments.Where(p =>  
                p.EntityType == AttachmentEntityType.WorkItemTemplate
                && allWorkItemTemplateIds.Contains(p.EntityId)
                && p.Type == AttachmentType.Image
            ).ToList();

            result.ForEach(p => p.workItems.ForEach(workItem => {
                workItem.TaskNumber = i++;
                var templateImage = allTemplateImages.Where(tem => tem.EntityId == workItem.WorkItemTemplateId).ToList();
                if (images.Any(w => w.EntityId == workItem.Id)
                || templateImage.Any())
                {
                    workItem.ImageUrls = images.Where(w => w.EntityId == workItem.Id).Select(w => w.Url).ToList();
                    workItem.ImageUrls.AddRange(templateImage.Select(t => t.Url));
                    
                }
            }));
            return result.ToList();
        }
     

        public IQueryable<WorkItem> GetAllPropertyWorkItems(int propertyId)
        {
            var workItems = _ctx.Properties
                .Where(p => p.Id == propertyId)
                .SelectMany(p => p.SectionList)
                .SelectMany(p=> p.WorkItemList);
            return workItems;
        }

        public AllocationModel UpdateMemberAllocation(string userName, int propertyId, int memberId, bool allocate)
        {
            var companyId = _companyRepository.GetCompanyFoAdminUser(userName).Id;
            AllocationModel result = null;

            if (allocate)
            {
                var property = _ctx.Properties.First(p => p.Id == propertyId);
                var cm = _ctx.CompanyMembers.First(p => p.CompanyId == companyId && p.MemberId == memberId);
                //create the allocation record
                PropertyAllocation newItem = new PropertyAllocation
                {
                    Property = property,
                    CompanyMember = cm,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                _ctx.Entry(newItem).State = EntityState.Added;
                result = new AllocationModel() {
                     Allocated = true,
                      Description = property.Description,
                       PropertyId = property.Id,
                        PropertyName = property.Name,
                };
            }
            else
            {
                //delete the allocation record
                var info = GetMemberProperty(companyId, propertyId, memberId).First();
                _ctx.PropertyAllocations.Remove(info);
            }
            _ctx.SaveChanges();
            return result;
        }

        public IQueryable<AllocationModel> GetMemberAllocation(string userName, int memberId)
        {
            var companyId = _companyRepository.GetCompanyFoAdminUser(userName).Id;
            return GetMemberPropertyAllocationInfo(companyId,memberId);

        }

        private IQueryable<AllocationModel> GetMemberPropertyAllocationInfo(int companyId, int memberId)
        {
            var info = from m in _ctx.Members
                       join cm in _ctx.CompanyMembers on m.Id equals cm.MemberId
                       join company in _ctx.Companies on cm.CompanyId equals company.Id
                       join cp in _ctx.PropertyCompanies on company.Id equals cp.CompanyId
                       join p in _ctx.Properties on cp.PropertyId equals p.Id


                       join pl in _ctx.PropertyAllocations on 
                       new { PropertyId = p.Id, CompanyMemberId = cm.Id } 
                       equals new { PropertyId = pl.PropertyId, CompanyMemberId = pl.CompanyMemberId } into a
                       from result in a.DefaultIfEmpty()


                       where (cm.Role == CompanyRole.Contractor) // contractor only
                       && m.Id == memberId
                       && company.Id == companyId

                       select new AllocationModel()
                       {
                           PropertyName = p.Name,
                           Allocated = a.Any(),
                           Description = p.Description,
                           PropertyId = p.Id
                       };
            return info;

        }

        private IQueryable<PropertyAllocation> GetMemberProperty(int companyId, int propertyId, int memberId)
        {
            var info = from m in _ctx.Members
                       join cm in _ctx.CompanyMembers on m.Id equals cm.MemberId
                       join company in _ctx.Companies on cm.CompanyId equals company.Id
                       join pl in _ctx.PropertyAllocations.DefaultIfEmpty() on cm.Id equals pl.CompanyMemberId
                       join p in _ctx.Properties on pl.PropertyId equals p.Id
                       where (cm.Role == CompanyRole.Contractor) // contractor only
                       && m.Id == memberId
                       && company.Id == companyId
                       && p.Id == propertyId
                       select pl;
                     
            return info;

        }

        private IQueryable<Property> GetClientProperties(int clientId)
        {
            var property = from client in _ctx.Clients
                           join cp in _ctx.ClientProperties on client.Id equals cp.ClientId
                           join p in _ctx.Properties on cp.PropertyId equals p.Id
                           where client.Id == clientId
                           select p;
            return property;


        }

        private IQueryable<Property> GetMemberProperties(int memberId)
        {
            var propertyViaAllocation = GetMemberPropertyViaAllocation(memberId);
            var propertyViaCompany = GetMemberPropertyViaCompany(memberId);
            return propertyViaAllocation.Union(propertyViaCompany);

        }

        //this is for company role -> contractor only
        private IQueryable<Property> GetMemberPropertyViaAllocation(int memberId)
        {
            

            var properties = _ctx.Members.Where(p => p.Id == memberId).SelectMany(p => p.CompanyMembers)
                .Where(p => p.Role == CompanyRole.Contractor ).SelectMany(p => p.PropertyAllocations).Select(p => p.Property);
            return properties;

        }

        //this is for company role -> admin and  defualt
        private IQueryable<Property> GetMemberPropertyViaCompany(int memberId)
        {
            var properties = from m in _ctx.Members
                             join cm in _ctx.CompanyMembers on m.Id equals cm.MemberId
                             join company in _ctx.Companies on cm.CompanyId equals company.Id
                             join cp in _ctx.PropertyCompanies on company.Id equals cp.CompanyId
                             join p in _ctx.Properties on cp.PropertyId equals p.Id
                             where (cm.Role == CompanyRole.Admin || cm.Role == CompanyRole.Default)
                             && m.Id == memberId
                            

                             select p;
            return properties;

        }

        public IQueryable<Client> GetPropertyOwnerClinet(int propertyId)
        {
           var ownerClient = _ctx.Properties.Where(p => p.Id == propertyId)
                .SelectMany(p => p.ClientProperties)
                .Where(p => p.Role == ClientRole.Owner).Select(p => p.Client);//there should be only one owner client for each property

            return ownerClient;


        }

        public IQueryable<Company> GetCompanyForProperty(int propertyID)
        {
            // get the company that this property has been assigned to.
            IQueryable<Company> companies =   _ctx.Properties.Where(p => p.Id == propertyID).SelectMany(p => p.PropertyCompanies).Select(p => p.Company);
            return companies;


        }

        public void CreatePropertyForWorkRequest(int messageId, PropertyModel model)
        {
            var message = _ctx.Messages.Find(messageId);
            if (message == null)
            {
                throw new Exception("Cannot find message with Id " + messageId);
            }
            if (!message.IsWaitingForResponse)
            {
                throw new Exception("Message already handled ");
            }

            if (message.PropertyId != null)
            {
                throw new Exception("Property already attached for this message");
            }

            if (model.Address == null)
            {
                throw new Exception("Address cannot be null");
            }

            Property newProperty = new Property
            {
                Name = model.Name,
                Address = new Address()
                {
                    City = model.Address.City,
                    Line1 = model.Address.Line1,
                    Line2 = model.Address.Line2,
                    Line3 = model.Address.Line3,
                    PostCode = model.Address.PostCode,
                    State = model.Address.State,
                    Suburb = model.Address.Suburb,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                },
                Description = model.Description,
                Condition = model.Condition,
                Narrative = model.Narrative,
                Comment = model.Comment,
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,

            };
            _ctx.Entry(newProperty).State = EntityState.Added;
            message.Property = newProperty;
            message.PropertyAddress = message.PropertyAddress + " (Address record created)";
            _ctx.Entry(message).State = EntityState.Modified;


            //add this property to the company
            PropertyCompany propertyCompanyNew = new PropertyCompany
            {
                Property = newProperty,
                CompanyId = message.CompanyId.Value,
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
            };
            _ctx.Entry(propertyCompanyNew).State = EntityState.Added;



            ClientProperty clientPropertyNew = new ClientProperty
            {
                ClientId = message.ClientId.Value,
                Property = newProperty,
                Confirmed = true,
                Role = ClientRole.Owner,
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
            };

            _ctx.Entry(clientPropertyNew).State = EntityState.Added;

            CreateDefaultSections(model.DefaultSections, newProperty, _ctx);

            _ctx.SaveChanges();

        }
        private List<Attachment> GetPropertyWorkItemsAttachments(int propertyId, string userName)
        {
            if (!CheckUserPermissionForProperty(propertyId, AttachmentEntityType.Property, userName))
                throw new Exception(
                    $"User {userName} has no permission to download attachment on {AttachmentEntityType.Property} with id {propertyId}");

            var workItemIdsForProperty = GetAllPropertyWorkItems(propertyId).Select(p => p.Id).ToList();

            if (workItemIdsForProperty.Any())
            {
                var allBlobs = _ctx.Attchments.Where(p =>  //add the type and entityId here just to make sure right attachment has been fetched.
                    p.EntityType == AttachmentEntityType.WorkItem
                    && workItemIdsForProperty.Contains(p.EntityId)
                ).ToList();
                return allBlobs;
            }
            return new List<Attachment>();


        }
        private bool CheckUserPermissionForProperty(int propertyId, AttachmentEntityType type, string userName)
        {
            var allowedProperties = GetPropertyForUser(userName).ToList().Select(p => p.Id);
            return  allowedProperties.Count(p => p == propertyId) == 1;//found
        }
    }
}