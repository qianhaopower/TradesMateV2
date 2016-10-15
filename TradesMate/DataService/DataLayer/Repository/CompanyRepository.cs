
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

    public class CompanyRepository : IDisposable
    {
        private EFDbContext _ctx;
      
        private UserManager<ApplicationUser> _userManager;      
        public CompanyRepository()
        {
            _ctx = new EFDbContext();          
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }

        


        public IQueryable<Property> GetCompanyProperties(int companyId)
        {
            // get the company that this property has been assigned to.
            IQueryable<Property> properties =   _ctx.Companies.Where(p => p.Id == companyId).SelectMany(p => p.PropertyCompanies).Select(p => p.Property);
            return properties;


        }


        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}