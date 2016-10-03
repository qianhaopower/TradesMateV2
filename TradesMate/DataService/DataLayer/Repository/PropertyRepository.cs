
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

    public class PropertyRepository : IDisposable
    {
        private EFDbContext _ctx;
        private EFDbContext _applicationContext;

        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public PropertyRepository()
        {
            _ctx = new EFDbContext();
            _applicationContext = new EFDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new EFDbContext()));
        }

        public   IQueryable<Property> GetPropertyForUser(string userName)
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

                    properties = _ctx.Properties.Where(p => p.ClientId == user.Client.Id);

                }else if (user.UserType == (int)UserType.Trade)
                {
                    var currentUserCompany = user.CompanyId;//should not be zero.

                   
                    properties = _ctx.Properties.Where(p => p.Companies.Select(z => z.Id).Contains(currentUserCompany));
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


        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}