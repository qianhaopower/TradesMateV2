
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


        private  Company GetCompanyFoAdminUser(string userName)
        {
            var _repo = new AuthRepository();

            //user must be admin.
            var userTask =  _repo.GetUserByUserName(userName);
            var user = userTask.Result;//happy to wait here.
            if (user.UserType != UserType.Trade)
                throw new Exception("Only member can view company members");


            _ctx.Entry(user).Reference(s => s.Member).Load();
            if (user.Member == null)
                throw new Exception("Only member can view company members");

            var company = user.Member.CompanyMembers.FirstOrDefault(p => p.Role == CompanyRole.Admin);

            if (company == null)
                throw new Exception("Only admin member can view company members");

            return company.Company;
        }

        public IQueryable<Member> GetMemberByUserName(string userName)
        {
            var companyId = GetCompanyFoAdminUser(userName).Id;


            var members = from com in _ctx.Companies
                          join cm in _ctx.CompanyMembers on com.Id equals cm.CompanyId
                          join mem in _ctx.Members on cm.MemberId equals mem.Id
                          where com.Id == companyId
                          select mem;

            return members;
        }



        public Company GetCompanyForCurrentUser(string userName) {
            var company = GetCompanyFoAdminUser(userName);
            return company;

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