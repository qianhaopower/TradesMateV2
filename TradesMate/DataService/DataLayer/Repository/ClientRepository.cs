
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
                    var currentUserCompanyId = user.CompanyId;//should not be zero.
                    // all the clients that has at least one property with that company
                   
                    clients = _ctx.Clients.Where(p => p.Properties.Any(z => z.Companies.Select(t => t.Id).Contains(currentUserCompanyId)));
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


        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}