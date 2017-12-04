

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

    public class SectionRepository : IDisposable
    {
        private EFDbContext _ctx;

        private UserManager<ApplicationUser> _userManager;
        public SectionRepository(EFDbContext ctx = null)
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

        internal List<WorkItem> GetSectionWorkItemList(string userName, int sectionId)
        {

            if (HasPermissionForSection(userName, sectionId))
            {
                return _ctx.Sections.Where(m => m.Id == sectionId).SelectMany(m => m.WorkItemList).ToList();
            }
            else
            {
                throw new Exception($"No permission to view section{sectionId} work items");
            }
        }
        internal Section GetSectionById(string userName,int sectionId)
        {

            if (HasPermissionForSection(userName, sectionId))
            {
                return _ctx.Sections.FirstOrDefault(s=> s.Id== sectionId);
            }
            else
            {
                throw new Exception($"No permission to view section{sectionId}");
            }
        }

        internal void DeleteSectionById(string userName, int sectionId)
        {

            if (HasPermissionForSection(userName, sectionId))
            {
               var toDelete =  _ctx.Sections.FirstOrDefault(s => s.Id == sectionId);
                if(toDelete != null)
                {
                    _ctx.Sections.Remove(toDelete);
                    _ctx.SaveChanges();
                }
            }
            else
            {
                throw new Exception($"No permission to delete section{sectionId}");
            }
        }
        internal Section CreateSection(string userName, SectionModel model)
        {

            if (HasPermissionForProperty(userName, model.PropertyId))
            {
                var newSection = Mapper.Map<SectionModel, Section>(model);
                newSection.AddedDateTime = DateTime.Now;
                newSection.ModifiedDateTime = DateTime.Now;
                _ctx.Sections.Add(newSection);
                _ctx.Entry(newSection).State = EntityState.Added;
                _ctx.SaveChanges();

                return newSection;
            }
            else
            {
                throw new Exception($"No permission to create section{model.PropertyId}");
            }
        }
        internal Section UpdateSection(string userName, SectionModel model)
        {

            if (HasPermissionForSection(userName, model.Id))
            {
                var section = _ctx.Sections.FirstOrDefault(s => s.Id == model.Id);
                section.ModifiedDateTime = DateTime.Now;
                section.Name = model.Name;
                section.Description = model.Description;
                section.Type = model.Type;
                _ctx.Entry(section).State = EntityState.Modified;
                _ctx.SaveChanges();
                return section;
            }
            else
            {
                throw new Exception($"No permission to view section{model.Id}");
            }
        }

        internal bool HasPermissionForSection(string userName, int sectionId)
        {
            var allowedPropertySections = new PropertyRepository(_ctx).GetPropertyForUser(userName).SelectMany(p => p.SectionList).Select(s => s.Id);
            return (allowedPropertySections.Any(p => p == sectionId));
        }
        internal bool HasPermissionForProperty(string userName, int propertyId)
        {
            var allowedProperty = new PropertyRepository(_ctx).GetPropertyForUser(userName);
            return (allowedProperty.Any(p => p.Id == propertyId));
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }

    }
}