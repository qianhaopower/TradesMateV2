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
    [RoutePrefix("api/workitemtemplates")]
    public class WorkItemTemplatesController : ApiController
    {
        private IAuthRepository _authRepo;
        private readonly IWorkItemTemplateRepository _workItemTempRepo;
        public WorkItemTemplatesController(IAuthRepository authRepo, IWorkItemTemplateRepository workItemTempRepo)
        {
            _authRepo = authRepo;
            _workItemTempRepo = workItemTempRepo;
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<WorkItemTemplateModel> GetWorkItemTemplates()
        {

            var workItemTemplates = _workItemTempRepo.GetWorkItemTemplateForUser(User.Identity.Name);

            var returnList = workItemTemplates.Select(Mapper.Map<WorkItemTemplate, WorkItemTemplateModel>).ToList();

            return returnList;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetWorkItemTemplateById(int id)
        {

            var workItemTemplate = _workItemTempRepo.GetWorkItemTemplateByIdForUser(User.Identity.Name, id);
            return Ok(Mapper.Map<WorkItemTemplate, WorkItemTemplateModel>(workItemTemplate));
        }

        [HttpPut]
        [Route("")]
        public async Task<WorkItemTemplateModel> UpdateWorkItemTemplate(WorkItemTemplateModel model)
		{

			 await _workItemTempRepo.UpdateWorkItemTemplateForUserAsync(User.Identity.Name, model);

			return model;
		}

		[HttpPost]
		[Route("")]
        public async Task<WorkItemTemplateModel> CreateWorkItemTemplate( WorkItemTemplateModel model)
		{

			await _workItemTempRepo.CreateWorkItemTemplateForUserAsync(User.Identity.Name, model);

			return model;
		}

		[HttpDelete]
		[Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteWorkItemTemplate(int id)
		{

			await _workItemTempRepo.DeleteWorkItemTemplateForUserAsync(User.Identity.Name, id);

			return StatusCode(HttpStatusCode.NoContent);
		}

		
    }
}
