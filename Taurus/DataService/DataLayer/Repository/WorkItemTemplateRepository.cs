
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

    public class WorkItemTemplateRepository : BaseRepository, IWorkItemTemplateRepository
    {

        public WorkItemTemplateRepository(EFDbContext ctx) : base(ctx)
        {

        }

        public List<WorkItemTemplate> GetWorkItemTemplateForUser(string userName)
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
            // only return type that current company has, like electrician
            var allowService = _ctx.CompanyServices.Where(p => p.CompanyId == companyId).Select(p => p.Type).ToList();

            list = list.Where(p => allowService.Contains(p.TradeWorkType)).ToList();

            return list;
        }

        public WorkItemTemplate GetWorkItemTemplateByIdForUser(string userName, int id)
        {
            var _repo = new AuthRepository(_ctx);

            var _companyRepo = new CompanyRepository(_ctx);
            // get all the workItemTemplate for that company
            var companyId = _companyRepo.GetCompanyFoAdminUser(userName).Id;
            var allowService = _ctx.CompanyServices.Where(p => p.CompanyId == companyId).Select(p => p.Type).ToList();

            var oneItem = _ctx.WorkItemTemplates.Where(p =>
            p.CompanyId == companyId
            && p.Id == id
            && allowService.Contains(p.TradeWorkType)).Single();
            return oneItem;
        }

        public async Task CreateWorkItemTemplateForUserAsync(string userName, WorkItemTemplateModel model)
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
				CompanyId = companyId,
				AddedDateTime = DateTime.Now,
				ModifiedDateTime = DateTime.Now,

			};
			_ctx.Entry(newItem).State = EntityState.Added;
			_ctx.SaveChanges();

		}

        public async Task UpdateWorkItemTemplateForUserAsync(string userName, int wormItemTemplateId, WorkItemTemplateModel model)
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

        public async Task DeleteWorkItemTemplateForUserAsync(string userName, int wormItemTemplateId)
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
    }
}