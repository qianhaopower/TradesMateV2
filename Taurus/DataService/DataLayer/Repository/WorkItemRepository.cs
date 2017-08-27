
using DataService.Entities;
using DataService.Infrastructure;
using DataService.Models;
using EntityFramework.Extensions;
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

    public class WorkItemRepository : IDisposable
    {
        private EFDbContext _ctx;

        private UserManager<ApplicationUser> _userManager;
        public WorkItemRepository(EFDbContext ctx = null)
        {
            if (ctx != null)
            {
                _ctx = ctx;
            }
            else
            {
                _ctx = new EFDbContext();
            }

            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }


        public IEnumerable<WorkItem> GetSectionWorkItems(int sectionId, string UserName)
        {
            var propertyId = _ctx.Sections.Single(p => p.Id == sectionId).PropertyId;
            var repo = new PropertyRepository(_ctx);
            var hasPermission = repo.GetPropertyForUser(UserName).Any(p => p.Id == propertyId);
            if (!hasPermission)
                throw new Exception(string.Format("No permission to view work items for section with Id {0}", sectionId));

            var companyMembers = from m in _ctx.Members
                                 join cm in _ctx.CompanyMembers on m.Id equals cm.MemberId
                                 join company in _ctx.Companies on cm.CompanyId equals company.Id
                                 join cp in _ctx.PropertyCompanies on company.Id equals cp.CompanyId
                                 join p in _ctx.Properties on cp.PropertyId equals p.Id
                                 join cms in _ctx.CompanyServices on company.Id equals cms.CompanyId
                                 join u in _ctx.Users on m equals u.Member
                                 where u.UserName == UserName
                                 && p.Id == propertyId

                                 select cm;
            List<TradeType> userAllowedServiceType = new List<TradeType>();
            if (companyMembers.Any(p => p.Role == CompanyRole.Admin))
            {
                var companyId = companyMembers.First(p => p.Role == CompanyRole.Admin).CompanyId;
                //this member is an admin for one company, cannot work for other compnays, just give all service types for that compnay
                userAllowedServiceType = _ctx.CompanyServices.Where(p => p.CompanyId == companyId).Select(p => p.Type).ToList();
            }
            else
            {
                //this user is defaulf or contract, need check for each company what permission he/she has
                companyMembers.ToList().ForEach(p =>
                {
                    userAllowedServiceType = userAllowedServiceType.Union(p.AllowedTradeTypes).ToList();
                });
            }


            var workItems = _ctx.Sections
                .Include(z => z.WorkItemList)
                .Single(p => p.Id == sectionId)
                .WorkItemList
                .Where(p => userAllowedServiceType.Contains(p.TradeWorkType))
                .ToList();

            return workItems;
        }
       
        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }

    }
}