
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

    public interface IWorkItemTemplateRepository : IBaseRepository
    {
        List<WorkItemTemplate> GetWorkItemTemplateForUser(string userName);

        WorkItemTemplate GetWorkItemTemplateByIdForUser(string userName, int id);

        Task CreateWorkItemTemplateForUserAsync(string userName, WorkItemTemplateModel model);

        Task UpdateWorkItemTemplateForUserAsync(string userName, WorkItemTemplateModel model);

        Task DeleteWorkItemTemplateForUserAsync(string userName, int wormItemTemplateId);
    }
}