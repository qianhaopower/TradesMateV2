
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

    public class ClientRepository : IDisposable
    {
        private EFDbContext _ctx;
        private EFDbContext _applicationContext;

        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public ClientRepository()
        {
            _ctx = new EFDbContext();
            _applicationContext = new EFDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new EFDbContext()));
        }


        //decide what clients can this user see. 
        //if the user is a client, then can only see client him/herself

        //if the user is a member, he/she can see all the clients whose property he/she has access to.

        public   IQueryable<Client> GetClientForUser(string userName)
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
                else if (user.UserType == (int)UserType.Trade)
                {
                    var properties = new PropertyRepository().GetPropertyForUser(userName);

                    clients = GetClientsForProperty(properties);
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

        public IQueryable<Client> GetClientsForProperty(IQueryable<Property> list)
        {
            var clients = from p in list
                          join cp in _ctx.ClientProperties on p.Id equals cp.PropertyId
                          join c in _ctx.Clients on cp.Id equals c.Id
                          select c;
            return clients;

        }


        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}