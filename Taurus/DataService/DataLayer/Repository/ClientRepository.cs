using DataService.Infrastructure;
using DataService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using Z.EntityFramework.Plus;

namespace EF.Data
{

    public class ClientRepository : BaseRepository, IClientRepository
    {
        public ClientRepository(EFDbContext ctx) :base(ctx)
        {
            
        }
        
        public Client GetClientForUser (string userId)
        {
            var user = _userManager.FindById(userId);
            if(user.UserType == UserType.Client)
            {
                _ctx.Entry(user).Reference(s => s.Client).Load();
                return _ctx.Clients.First(p => p.Id == user.Client.Id);
            }else
            {
                throw new Exception("User is not a client");
            }
           
        }

        public Member GetMemberForUser(string userId)
        {
            var user = _userManager.FindById(userId);
            if (user.UserType == UserType.Trade)
            {
                _ctx.Entry(user).Reference(s => s.Member).Load();
                return _ctx.Members.First(p => p.Id == user.Member.Id);
            }
            else
            {
                throw new Exception("User is not a member");
            }

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
                    var properties = new PropertyRepository(_ctx).GetPropertyForUser(userName);

                    clients = GetClientsForProperty(properties);

                    //need get all the client that has a work request. 
                    var clientHasRequest = new MessageRepository(_ctx).GetClientThatHasMessageForUser(userName);
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

        public void DeleteClient(string userName, int clientId)
        {
            var _repo = new AuthRepository(_ctx);
            var isUserAdmin = _repo.isUserAdmin(userName);
            if (!isUserAdmin)
            {
                throw new Exception("Only Admin user can delete client");
            }

            var clientToDelete = GetAccessibleClientForUser(userName).FirstOrDefault(c => c.Id == clientId);
            if(clientToDelete != null)
            {
                _ctx.Clients.Where(c => c.Id == clientToDelete.Id).Delete();
                _ctx.SaveChanges();
            }
        }

        private IQueryable<Client> GetClientsForProperty(IQueryable<Property> list)
        {
            var clients = from p in list
                          join cp in _ctx.ClientProperties on p.Id equals cp.PropertyId
                          join c in _ctx.Clients on cp.Id equals c.Id
                          select c;
            return clients;

        }
    }
}