using System.Data;
using System.Linq;
using System.Web.Http;
using EF.Data;
using AutoMapper;
using DataService.Models;

namespace DataService.Controllers
{
    [Authorize]
    public class SectionsController : ApiController
    {
        private IAuthRepository _authRepo;
        private ISectionRepository _sectionRepo;
        public SectionsController(IAuthRepository authRepo, ISectionRepository sectionRepo)
        {
            _authRepo = authRepo;
            _sectionRepo = sectionRepo;
        }
        [HttpGet]
        public IHttpActionResult GetSection(int key)
        {
            
            var section = _sectionRepo.GetSectionById(User.Identity.Name, key);
            return Ok(Mapper.Map<Section, SectionModel>(section));
        }

        [HttpPost]
        public IHttpActionResult UpdateSection(SectionModel section)
        {
          
            var sectionReasult = _sectionRepo.UpdateSection(User.Identity.Name, section);
            return Ok(Mapper.Map<Section, SectionModel>(sectionReasult));
        }



        [HttpPost]
        public IHttpActionResult Create(SectionModel section)
        {
           
            var sectionReasult = _sectionRepo.CreateSection(User.Identity.Name, section);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int sectionId)
        {
            _sectionRepo.DeleteSectionById(User.Identity.Name, sectionId);
            return Ok();
        }
  
        [HttpGet]
        public IHttpActionResult GetWorkItemList(int sectionId)
        {
           
            var reasult = _sectionRepo.GetSectionWorkItemList(User.Identity.Name, sectionId).Select(Mapper.Map<WorkItem, WorkItemModel>).ToList(); ;
            return Ok(reasult);
        }
   
    }
}
