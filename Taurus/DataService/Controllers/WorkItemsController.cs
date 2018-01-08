using System.Data;
using System.Linq;
using System.Web.Http;
using EF.Data;
using AutoMapper;
using DataService.Models;

namespace DataService.Controllers
{

    [Authorize]
    [RoutePrefix("api/workitems")]
    public class WorkItemsController : ApiController
    {

        private readonly IAuthRepository _authRepo;
        private readonly IWorkItemRepository _workItemRepo;
        public WorkItemsController(IAuthRepository authRepo, IWorkItemRepository workItemRepo)
        {
            _authRepo = authRepo;
            _workItemRepo = workItemRepo;
        }
      

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetWorkItem(int id)
        {
            var workItem = _workItemRepo.GetWorkItemById(User.Identity.Name, id);
            return Ok(Mapper.Map<WorkItem, WorkItemModel>(workItem));
        }


        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(WorkItemModel model)
        {
            var workItem = _workItemRepo.CreateWorkItem(User.Identity.Name, model);
            return Ok(Mapper.Map<WorkItem, WorkItemModel>(workItem));
        }
        [HttpPut]
        [Route("")]
        public IHttpActionResult Update(WorkItemModel model)
        {
            var workItem = _workItemRepo.UpdateWorkItem(User.Identity.Name, model);
            return Ok(Mapper.Map<WorkItem, WorkItemModel>(workItem));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            _workItemRepo.DeleteWorkItemById(User.Identity.Name, id);
            return Ok();
        }

    }




}
