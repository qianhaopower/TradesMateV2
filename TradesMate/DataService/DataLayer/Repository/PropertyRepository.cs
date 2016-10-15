
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

                    properties = GetClientProperties(user.Client.Id);

                }
                else if (user.UserType == (int)UserType.Trade)
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
                .Where(p => p.Role == CompanyRole.Contractor).SelectMany(p => p.PropertyAllocations).Select(p => p.Property);
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
                             where (cm.Role == CompanyRole.Admin || cm.Role ==CompanyRole.Default)
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


        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}