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
        [HttpGet]
        public IHttpActionResult GetWorkItemBySectionId(int sectionId)
        {
            var repo = new WorkItemRepository();
            var workItems = repo.GetSectionWorkItems(User.Identity.Name, sectionId);
            return Ok(workItems.Select(Mapper.Map<WorkItem, WorkItemModel>));
        }

        [HttpGet]
        public IHttpActionResult GetWorkItem(int workItemId)
        {
            var repo = new WorkItemRepository();
            var workItem = repo.GetWorkItemById(User.Identity.Name, workItemId);
            return Ok(Mapper.Map<WorkItem, WorkItemModel>(workItem));
        }


        [HttpPost]
        public IHttpActionResult Create(WorkItemModel model)
        {
            var repo = new WorkItemRepository();
            var workItem = repo.CreateWorkItem(User.Identity.Name, model);
            return Ok(Mapper.Map<WorkItem, WorkItemModel>(workItem));
        }
        [HttpPost]
        public IHttpActionResult Update(WorkItemModel model)
        {
            var repo = new WorkItemRepository();
            var workItem = repo.UpdateWorkItem(User.Identity.Name, model);
            return Ok(Mapper.Map<WorkItem, WorkItemModel>(workItem));
        }

        [HttpDelete]
        public IHttpActionResult Delete(int workItemId)
        {
            var repo = new WorkItemRepository();
            repo.DeleteWorkItemById(User.Identity.Name, workItemId);
            return Ok();
        }

    }




}
