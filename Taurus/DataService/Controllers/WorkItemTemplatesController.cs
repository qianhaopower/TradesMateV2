using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Http;
using EF.Data;
using DataService.Models;
using AutoMapper;
using System.Threading.Tasks;

namespace DataService.Controllers
{

    [Authorize]
	public class WorkItemTemplatesController : ApiController
    {
        private IAuthRepository _authRepo;
        private IWorkItemTemplateRepository _workItemTempRepo;
        public WorkItemTemplatesController(IAuthRepository authRepo, IWorkItemTemplateRepository workItemTempRepo)
        {
            _authRepo = authRepo;
            _workItemTempRepo = workItemTempRepo;
        }

        [HttpGet]
        public IEnumerable<WorkItemTemplateModel> GetWorkItemTemplates()
        {

            var workItemTemplates = _workItemTempRepo.GetWorkItemTemplateForUser(User.Identity.Name);

            var returnList = workItemTemplates.Select(Mapper.Map<WorkItemTemplate, WorkItemTemplateModel>).ToList();

            return returnList;
        }


        [HttpGet]
        public IHttpActionResult GetWorkItemTemplateById(int templateId)
        {

            var workItemTemplate = _workItemTempRepo.GetWorkItemTemplateByIdForUser(User.Identity.Name, templateId);
            return Ok(Mapper.Map<WorkItemTemplate, WorkItemTemplateModel>(workItemTemplate));
        }




        [HttpPatch]
        public async Task<WorkItemTemplateModel> UpdateWorkItemTemplate(int templateId, WorkItemTemplateModel model)
		{

			 await _workItemTempRepo.UpdateWorkItemTemplateForUserAsync(User.Identity.Name, templateId, model);

			return model;
		}



		[HttpPost]
        public async Task<WorkItemTemplateModel> CreateWorkItemTemplate( WorkItemTemplateModel model)
		{

			await _workItemTempRepo.CreateWorkItemTemplateForUserAsync(User.Identity.Name, model);

			return model;
		}



		[HttpDelete]
		public async Task<IHttpActionResult> DeleteWorkItemTemplate(int templateId)
		{

			await _workItemTempRepo.DeleteWorkItemTemplateForUserAsync(User.Identity.Name, templateId);

			return StatusCode(HttpStatusCode.NoContent);
		}

		
    }
}
