using System.Data;
using System.Linq;
using System.Web.Http;
using EF.Data;
using AutoMapper;
using DataService.Models;

namespace DataService.Controllers
{

    [Authorize]
    public class WorkItemsController : ApiController
    {

        private IAuthRepository _authRepo;
        private IWorkItemRepository _workItemRepo;
        public WorkItemsController(IAuthRepository authRepo, IWorkItemRepository workItemRepo)
        {
            _authRepo = authRepo;
            _workItemRepo = workItemRepo;
        }
        [HttpGet]
        public IHttpActionResult GetWorkItemBySectionId(int sectionId)
        {
            var workItems = _workItemRepo.GetSectionWorkItems(User.Identity.Name, sectionId);
            return Ok(workItems.Select(Mapper.Map<WorkItem, WorkItemModel>));
        }

        [HttpGet]
        public IHttpActionResult GetWorkItem(int workItemId)
        {
            var workItem = _workItemRepo.GetWorkItemById(User.Identity.Name, workItemId);
            return Ok(Mapper.Map<WorkItem, WorkItemModel>(workItem));
        }


        [HttpPost]
        public IHttpActionResult Create(WorkItemModel model)
        {
            var workItem = _workItemRepo.CreateWorkItem(User.Identity.Name, model);
            return Ok(Mapper.Map<WorkItem, WorkItemModel>(workItem));
        }
        [HttpPost]
        public IHttpActionResult Update(WorkItemModel model)
        {
            var workItem = _workItemRepo.UpdateWorkItem(User.Identity.Name, model);
            return Ok(Mapper.Map<WorkItem, WorkItemModel>(workItem));
        }

        [HttpDelete]
        public IHttpActionResult Delete(int workItemId)
        {
            _workItemRepo.DeleteWorkItemById(User.Identity.Name, workItemId);
            return Ok();
        }

    }




}
