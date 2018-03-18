

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

    public interface ISectionRepository : IBaseRepository
    {
        List<WorkItem> GetSectionWorkItemList(string userName, int sectionId);
        Section GetSectionById(string userName, int sectionId);

        void DeleteSectionById(string userName, int sectionId);
        Section CreateSection(string userName, SectionModel model);
        Section UpdateSection(string userName, SectionModel model);

        bool HasPermissionForSection(string userName, int sectionId);
        bool HasPermissionForProperty(string userName, int propertyId);

    }
}