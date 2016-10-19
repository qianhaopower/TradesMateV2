
using DataService.Entities;
using DataService.Infrastructure;
using DataService.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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


        internal  CompanyRole UpdateCompanyMemberRole(string userName ,int memberId, string role)
        {
            CompanyRole roleParsed;
            bool roleValid = Enum.TryParse<CompanyRole>(role, out roleParsed);
            if (roleValid ==false)
            {
                throw new Exception(string.Format("{0} is not a valid role name", role));
            }

            if(roleParsed == CompanyRole.Admin)
            {
                throw new Exception("Cannot assign Admin role");
            }
            if (roleParsed == CompanyRole.Contractor)
            {
                //business logic
                //nothing to do here, automaticly lost permission on property via compnay
            }
            var _repo = new AuthRepository(_ctx);
            var isUserAdminTask = _repo.isUserAdmin(userName);

            // For Task (not Task<T>): will block until the task is completed...
            isUserAdminTask.RunSynchronously();
            if (isUserAdminTask.Result == false)
            {
                throw new Exception("Only admin can update compnay members role");
            }

         

            //start from here.
            if (roleParsed == CompanyRole.Default)
            {
                var companyId = GetCompanyFoAdminUser(userName).Id;
                var otherCompanyInfo = GetMemberInfoOutsideCompany(companyId, memberId);
                var inOtherCompanyAsAdmin = otherCompanyInfo.Any(p=> p.CompanyMember.Role == CompanyRole.Admin);
                if (inOtherCompanyAsAdmin)
                {
                    throw new Exception("Member is the admin of other company, cannot assign role in this company.");
                }

                var inOtherCompanyAsDefault = otherCompanyInfo.Any(p => p.CompanyMember.Role == CompanyRole.Default);//lost default role in other company



                //business logic
                //need to check if the member being set is member of any other compnay
                //if yes, mark him/her as contractor for all other company, ask first. 

                //if no, mark him/her as default for this compnay, delete all property allocation for this company.
            }


         
           return  UpdateCompanyMemberRole(userName, memberId, role);



        }


        public IQueryable<MemberInfo> GetMemberInfoOutsideCompany(int companyId, int memberId )
        {
           // get all the memberInfo 


            var members = from com in _ctx.Companies
                          join cm in _ctx.CompanyMembers on com.Id equals cm.CompanyId
                          join mem in _ctx.Members on cm.MemberId equals mem.Id
                          join user in _ctx.Users on mem equals user.Member
                          where com.Id != companyId && mem.Id == memberId 
                          select new MemberInfo
                          {
                              Member = mem,//member record
                              CompanyMember = cm,//join record
                              Company = com,
                              User = user,
                          };

            return members;
        }

        private CompanyRole UpdateCompanyMemberRole(string userName,  int memberId, CompanyRole role)
        {
            var memberInfo = GetMemberByUserNameQuery(userName, memberId);

            if (memberInfo.Count() > 1)
            {
                throw new Exception("Multiple member found with ID" + memberId);
            }
           var cmRecord =  _ctx.CompanyMembers.Where(p => p.Company == memberInfo.First().Company && p.Member == memberInfo.First().Member).First();
            cmRecord.Role = role;

            _ctx.Entry(cmRecord).State = EntityState.Modified;

            _ctx.SaveChanges();
            return role;

        } 

      
        private   Company GetCompanyFoAdminUser(string userName)
        {
            var _repo = new AuthRepository(_ctx);

            //user must be admin.
            var user =    _repo.GetUserByUserName(userName);
           
            if (user.UserType != UserType.Trade)
                throw new Exception("Only member can view company members");


            _ctx.Entry(user).Reference(s => s.Member).Load();
           
            if (user.Member == null)
                throw new Exception("Only member can view company members");

            _ctx.Entry(user.Member).Collection(s => s.CompanyMembers).Load();
            var company = user.Member.CompanyMembers.ToList().FirstOrDefault(p => p.Role == CompanyRole.Admin);

            if (company == null)
                throw new Exception("Only admin member can view company members");


            _ctx.Entry(company).Reference(s => s.Company).Load();
            return company.Company;
        }

        public IQueryable<MemberInfo> GetMemberByUserNameQuery(string userName, int? memberId = null)
        {
            var companyId = GetCompanyFoAdminUser(userName).Id;


            var members = from com in _ctx.Companies
                          join cm in _ctx.CompanyMembers on com.Id equals cm.CompanyId
                          join mem in _ctx.Members on cm.MemberId equals mem.Id
                          join user in  _ctx.Users on mem equals user.Member
                          where com.Id == companyId && ( memberId.HasValue ? mem.Id == memberId : true)
                          select new MemberInfo
                          {
                             Member =  mem,//member record
                            CompanyMember =  cm,//join record
                            Company = com,
                            User = user,
                           
                          };

            return members;
        }
        public IQueryable<MemberModel> GetMemberByUserName(string userName, int? memberId = null)
        {
            var result = GetMemberByUserNameQuery(userName, memberId);
            return result.Select(p => new MemberModel
            {
                FirstName = p.Member.FirstName,
                LastName = p.Member.LastName,
                Email = p.Member.Email,
                MemberRole = p.CompanyMember.Role.ToString(),
                MemberId = p.Member.Id,
                Username = p.User.UserName,

            });
                
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