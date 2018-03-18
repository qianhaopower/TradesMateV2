
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

        private readonly IAuthRepository _authRepo;
        private readonly ICompanyRepository _companyRepository;
        public WorkItemTemplateRepository(EFDbContext ctx, ApplicationUserManager manager, IAuthRepository authRepo,  ICompanyRepository companyRepository) : base(ctx, manager)
        {
            _authRepo = authRepo;
            _companyRepository = companyRepository;
        }

        public List<WorkItemTemplate> GetWorkItemTemplateForUser(string userName)
        {
            // get all the workItemTemplate for that company
            var companyId = _companyRepository.GetCompanyForUser(userName).Id;

            var list = _ctx.WorkItemTemplates.Where(p => p.CompanyId == companyId).ToList();
            // only return type that current company has, like electrician
            var allowService = _ctx.CompanyServices.Where(p => p.CompanyId == companyId).Select(p => p.Type).ToList();

            list = list.Where(p => allowService.Contains(p.TradeWorkType)).ToList();

            return list;
        }

        public WorkItemTemplate GetWorkItemTemplateByIdForUser(string userName, int id)
        {
            // get all the workItemTemplate for that company
            var companyId = _companyRepository.GetCompanyFoAdminUser(userName).Id;
            var allowService = _ctx.CompanyServices.Where(p => p.CompanyId == companyId).Select(p => p.Type).ToList();

            var oneItem = _ctx.WorkItemTemplates.Single(p => p.CompanyId == companyId
            && p.Id == id
            && allowService.Contains(p.TradeWorkType));
            return oneItem;
        }

        public async Task CreateWorkItemTemplateForUserAsync(string userName, WorkItemTemplateModel model)
		{
			var isUserAdminTask = await _authRepo.IsUserAdminAsync(userName);


			if (isUserAdminTask == false)
			{
				throw new Exception("Only admin can create work item templates");
			}
			// get all the workItemTemplate for that company
			var companyId = _companyRepository.GetCompanyFoAdminUser(userName).Id;

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

        public async Task UpdateWorkItemTemplateForUserAsync(string userName, WorkItemTemplateModel model)
		{
			var isUserAdminTask = await _authRepo.IsUserAdminAsync(userName);

			if (isUserAdminTask == false)
			{
				throw new Exception("Only admin can update work item templates");
			}
			// get all the workItemTemplate for that company
			var companyId = _companyRepository.GetCompanyFoAdminUser(userName).Id;

			var workItemTemplate = _ctx.WorkItemTemplates.Single(p => p.Id == model.Id && p.CompanyId == companyId);


			workItemTemplate.Name = model.Name;
			workItemTemplate.Description = model.Description;
			workItemTemplate.TradeWorkType = model.TradeWorkType;
			workItemTemplate.TemplateType = model.TemplateType;
			workItemTemplate.CompanyId = model.CompanyId;
			workItemTemplate.ModifiedDateTime = DateTime.Now;

			
			_ctx.Entry(workItemTemplate).State = EntityState.Modified;
			_ctx.SaveChanges();
			

		}

        public async Task DeleteWorkItemTemplateForUserAsync(string userName, int wormItemTemplateId)
		{
			var isUserAdminTask = await _authRepo.IsUserAdminAsync(userName);

			if (isUserAdminTask == false)
			{
				throw new Exception("Only admin can delete work item templates");
			}
			// get all the workItemTemplate for that company
			var companyId = _companyRepository.GetCompanyFoAdminUser(userName).Id;

			var workItemTemplate = _ctx.WorkItemTemplates.Single(p => p.Id == wormItemTemplateId && p.CompanyId== companyId);

			_ctx.WorkItemTemplates.Remove(workItemTemplate);
			_ctx.SaveChanges();
		}

    }
}