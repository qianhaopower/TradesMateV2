using AutoMapper;
using DataService.Infrastructure;
using DataService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EF.Data
{

    public class WorkItemRepository : BaseRepository, IWorkItemRepository
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IPropertyRepository _propertyRepo;
        public WorkItemRepository(EFDbContext ctx, ApplicationUserManager manager, ISectionRepository sectionRepository, IPropertyRepository propertyRepo) : base(ctx, manager)
        {
            _sectionRepository = sectionRepository;
            _propertyRepo = propertyRepo;
        }
        public IEnumerable<WorkItem> GetSectionWorkItems(string userName, int sectionId)
        {
            var propertyId = _ctx.Sections.Single(p => p.Id == sectionId).PropertyId;
            var hasPermission = _propertyRepo.GetPropertyForUser(userName).Any(p => p.Id == propertyId);
            if (!hasPermission)
                throw new Exception($"No permission to view work items for section with Id {sectionId}");
            var user = this._userManager.FindByName(userName);
            _ctx.Entry(user).Reference(s => s.Member).Load();
            if (user.Client != null)
            {
                    var workItemsForclient = _ctx.Sections
                        .Include(z => z.WorkItemList)
                        .Single(p => p.Id == sectionId)
                        .WorkItemList
                        .ToList();

                    return workItemsForclient;
            }

            var companyMembers = from m in _ctx.Members
                                 join cm in _ctx.CompanyMembers on m.Id equals cm.MemberId
                                 join company in _ctx.Companies on cm.CompanyId equals company.Id
                                 join cp in _ctx.PropertyCompanies on company.Id equals cp.CompanyId
                                 join p in _ctx.Properties on cp.PropertyId equals p.Id
                                 join cms in _ctx.CompanyServices on company.Id equals cms.CompanyId
                                 join u in _ctx.Users on m equals u.Member
                                 where u.UserName == userName
                                 && p.Id == propertyId

                                 select cm;
            List<TradeType> userAllowedServiceType = new List<TradeType>();
            if (companyMembers.Any(p => p.Role == CompanyRole.Admin || p.Role == CompanyRole.Default))
            {
                var companyId = companyMembers.First(p => p.Role == CompanyRole.Admin || p.Role == CompanyRole.Default).CompanyId;
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
        public WorkItem GetWorkItemById(string userName, int workItemId)
        {
            var workItem = _ctx.WorkItems.FirstOrDefault(w => w.Id == workItemId);
            if (workItem != null && _sectionRepository.HasPermissionForSection(userName, workItem.SectionId))
            {
                return workItem;
            }
            else
            {
                throw new Exception($"No permission to view workitem with id {workItemId}");
            }
        }
        public WorkItem CreateWorkItem(string userName, WorkItemModel model)
        {
            if (_sectionRepository.HasPermissionForSection(userName, model.SectionId))
            {
                var newWorkItem = Mapper.Map<WorkItemModel, WorkItem>(model);//AutoMapper should have done it all
                newWorkItem.AddedDateTime = DateTime.Now;
                newWorkItem.ModifiedDateTime = DateTime.Now;
                //newWorkItem.Name = model.Name;
                //newWorkItem.Quantity = model.Quantity;
                //newWorkItem.Description = model.Description;
                //newWorkItem.WorkItemTemplateId = model.WorkItemTemplateId;
                //newWorkItem.TradeWorkType = model.TradeWorkType;
                //newWorkItem.SectionId = model.SectionId;
                //newWorkItem.Status = model.Status;
                _ctx.WorkItems.Add(newWorkItem);
                _ctx.Entry(newWorkItem).State = EntityState.Added;
                _ctx.SaveChanges();

                return newWorkItem;
            }
            else
            {
                throw new Exception($"No permission to create workitemf for section {model.SectionId}");
            }
        }
        public WorkItem UpdateWorkItem(string userName, WorkItemModel model)
        {
            if (_sectionRepository.HasPermissionForSection(userName, model.SectionId))
            {
                var toEdidWorkItem = _ctx.WorkItems.FirstOrDefault(w => w.Id == model.Id);
                if (toEdidWorkItem != null)
                {
                    toEdidWorkItem.ModifiedDateTime = DateTime.Now;
                    toEdidWorkItem.Name = model.Name;
                    toEdidWorkItem.Quantity = model.Quantity;
                    toEdidWorkItem.Description = model.Description;
                    toEdidWorkItem.WorkItemTemplateId = model.WorkItemTemplateId;
                    toEdidWorkItem.TradeWorkType = model.TradeWorkType;
                    toEdidWorkItem.SectionId = model.SectionId;
                    toEdidWorkItem.Status = (WorkItemStatus)Enum.Parse(typeof(WorkItemStatus), model.Status);
                    _ctx.Entry(toEdidWorkItem).State = EntityState.Modified;
                    _ctx.SaveChanges();
                }
                return toEdidWorkItem;
            }
            else
            {
                throw new Exception($"No permission to create workitemf for section {model.SectionId}");
            }
        }
        public void DeleteWorkItemById(string userName, int workItemId)
        {
            var workItem = _ctx.WorkItems.FirstOrDefault(w => w.Id == workItemId);
            if (workItem != null && _sectionRepository.HasPermissionForSection(userName, workItem.SectionId))
            {
                _ctx.WorkItems.Remove(workItem);
                _ctx.SaveChanges();
            }
            else
            {
                throw new Exception($"No permission to delete workitem with id {workItemId}");
            }
        }
    }
}