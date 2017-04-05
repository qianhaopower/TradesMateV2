using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using EF.Data;
using DataService.Models;
using AutoMapper;
using System.Threading.Tasks;

namespace DataService.Controllers
{
	
	[Authorize]
	public class WorkItemTemplatesController : ApiController
    {


		[HttpGet]
		private async Task<IEnumerable<WorkItemTemplateModel>> GetWorkItemTemplates()
		{

			var workItemTemplates = await new WorkItemTemplateRepository().GetWorkItemTemplateForUserAsync(User.Identity.Name);

			var returnList = workItemTemplates.Select(Mapper.Map<WorkItemTemplate, WorkItemTemplateModel>).ToList();

			return returnList;
		}



		[HttpPatch]
		private async Task<WorkItemTemplateModel> UpdateWorkItemTemplates(int id, WorkItemTemplateModel model)
		{

			 await new WorkItemTemplateRepository().UpdateWorkItemTemplateForUserAsync(User.Identity.Name, id, model);

			return model;
		}



		[HttpPost]
		private async Task<WorkItemTemplateModel> CreateWorkItemTemplates( WorkItemTemplateModel model)
		{

			await new WorkItemTemplateRepository().CreateWorkItemTemplateForUserAsync(User.Identity.Name, model);

			return model;
		}



		[HttpDelete]
		private async Task<IHttpActionResult> DeleteWorkItemTemplates(int id)
		{

			await new WorkItemTemplateRepository().DeleteWorkItemTemplateForUserAsync(User.Identity.Name,id);

			return StatusCode(HttpStatusCode.NoContent);
		}

		
    }
}
