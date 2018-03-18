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

    public interface IWorkItemRepository : IBaseRepository
    {
        IEnumerable<WorkItem> GetSectionWorkItems(string UserName, int sectionId);
        WorkItem GetWorkItemById(string userName, int workItemId);
        WorkItem CreateWorkItem(string userName, WorkItemModel model);
        WorkItem UpdateWorkItem(string userName, WorkItemModel model);
        void DeleteWorkItemById(string userName, int workItemId);
    }
}