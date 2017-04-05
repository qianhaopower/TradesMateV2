
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

    public class WorkItemTemplateRepository : IDisposable
    {
        private EFDbContext _ctx;
      
        public WorkItemTemplateRepository(EFDbContext ctx = null)
        {
            if (ctx != null)
            {
                _ctx = ctx;
            }
            else
            {
                _ctx = new EFDbContext();
            }
           
        }

		public async Task<List<WorkItemTemplate>> GetWorkItemTemplateForUserAsync(string userName)
		{
			var _repo = new AuthRepository(_ctx);
			//var isUserAdminTask = await _repo.isUserAdminAsync(userName);

			
			//if (isUserAdminTask == false)
			//{
			//	throw new Exception("Only admin can get templates");
			//}
			var _companyRepo = new CompanyRepository(_ctx);
			// get all the workItemTemplate for that company
			var companyId = _companyRepo.GetCompanyFoAdminUser(userName).Id;

			var list = _ctx.WorkItemTemplates.Where(p => p.CompanyId == companyId).ToList();

			return list;
		}

		internal async Task CreateWorkItemTemplateForUserAsync(string userName, WorkItemTemplateModel model)
		{
			var _repo = new AuthRepository(_ctx);
			var isUserAdminTask = await _repo.isUserAdminAsync(userName);


			if (isUserAdminTask == false)
			{
				throw new Exception("Only admin can create work item templates");
			}
			var _companyRepo = new CompanyRepository(_ctx);
			// get all the workItemTemplate for that company
			var companyId = _companyRepo.GetCompanyFoAdminUser(userName).Id;

			WorkItemTemplate newItem = new WorkItemTemplate
			{
				Name = model.Name,
			    Description = model.Description,
				TradeWorkType= model.TradeWorkType,
				TemplateType = model.TemplateType,
				CompanyId = model.CompanyId,
				AddedDateTime = DateTime.Now,
				ModifiedDateTime = DateTime.Now,

			};
			_ctx.Entry(newItem).State = EntityState.Added;
			_ctx.SaveChanges();

		}

		internal async Task UpdateWorkItemTemplateForUserAsync(string userName, int wormItemTemplateId, WorkItemTemplateModel model)
		{
			var _repo = new AuthRepository(_ctx);
			var isUserAdminTask = await _repo.isUserAdminAsync(userName);

			if (isUserAdminTask == false)
			{
				throw new Exception("Only admin can update work item templates");
			}
			var _companyRepo = new CompanyRepository(_ctx);
			// get all the workItemTemplate for that company
			var companyId = _companyRepo.GetCompanyFoAdminUser(userName).Id;

			var workItemTemplate = _ctx.WorkItemTemplates.Single(p => p.Id == wormItemTemplateId && p.CompanyId == companyId);


			workItemTemplate.Name = model.Name;
			workItemTemplate.Description = model.Description;
			workItemTemplate.TradeWorkType = model.TradeWorkType;
			workItemTemplate.TemplateType = model.TemplateType;
			workItemTemplate.CompanyId = model.CompanyId;
			workItemTemplate.AddedDateTime = DateTime.Now;
			workItemTemplate.ModifiedDateTime = DateTime.Now;

			
			_ctx.Entry(workItemTemplate).State = EntityState.Modified;
			_ctx.SaveChanges();
			

		}



		internal async Task DeleteWorkItemTemplateForUserAsync(string userName, int wormItemTemplateId)
		{
			var _repo = new AuthRepository(_ctx);
			var isUserAdminTask = await _repo.isUserAdminAsync(userName);

			if (isUserAdminTask == false)
			{
				throw new Exception("Only admin can delete work item templates");
			}
			var _companyRepo = new CompanyRepository(_ctx);
			// get all the workItemTemplate for that company
			var companyId = _companyRepo.GetCompanyFoAdminUser(userName).Id;

			var workItemTemplate = _ctx.WorkItemTemplates.Single(p => p.Id == wormItemTemplateId && p.CompanyId== companyId);

			_ctx.WorkItemTemplates.Remove(workItemTemplate);
			_ctx.SaveChanges();
		}








		public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}