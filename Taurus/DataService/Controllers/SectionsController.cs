using System.Data;
using System.Linq;
using System.Web.Http;
using EF.Data;
using AutoMapper;
using DataService.Models;

namespace DataService.Controllers
{
    [Authorize]
    [RoutePrefix("api/sections")]
    public class SectionsController : ApiController
    {
        private IAuthRepository _authRepo;
        private readonly ISectionRepository _sectionRepo;
        private readonly IWorkItemRepository _workItemRepo;
        public SectionsController(IAuthRepository authRepo, ISectionRepository sectionRepo, IWorkItemRepository workItemRepo)
        {
            _authRepo = authRepo;
            _sectionRepo = sectionRepo;
            _workItemRepo = workItemRepo;
        }
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetSection(int id)
        {
            var section = _sectionRepo.GetSectionById(User.Identity.Name, id);
            return Ok(Mapper.Map<Section, SectionModel>(section));
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult UpdateSection(SectionModel section)
        {
          
            var sectionReasult = _sectionRepo.UpdateSection(User.Identity.Name, section);
            return Ok(Mapper.Map<Section, SectionModel>(sectionReasult));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(SectionModel section)
        {
           
            var sectionReasult = _sectionRepo.CreateSection(User.Identity.Name, section);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            _sectionRepo.DeleteSectionById(User.Identity.Name, id);
            return Ok();
        }
  
        //[HttpGet]
        //[Route("{id:int}/workitems")]
        //public IHttpActionResult GetWorkItemList(int id)
        //{
        //    var reasult = _sectionRepo.GetSectionWorkItemList(User.Identity.Name, id).Select(Mapper.Map<WorkItem, WorkItemModel>).ToList(); ;
        //    return Ok(reasult);
        //}
        [HttpGet]
        [Route("{sectionId:int}/workitems")]
        public IHttpActionResult GetWorkItemBySectionId(int sectionId)
        {
            var workItems = _workItemRepo.GetSectionWorkItems(User.Identity.Name, sectionId);
            return Ok(workItems.Select(Mapper.Map<WorkItem, WorkItemModel>));
        }

    }
}
