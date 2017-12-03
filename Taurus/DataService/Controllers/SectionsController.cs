using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using EF.Data;
using AutoMapper;
using DataService.Models;

namespace DataService.Controllers
{
    [Authorize]
    public class SectionsController : ApiController
    {

        [HttpGet]
        public IHttpActionResult GetSection(int key)
        {
            var repo = new SectionRepository();
            var section = repo.GetSectionById(User.Identity.Name, key);
            return Ok(Mapper.Map<Section, SectionModel>(section));
        }

        [HttpPost]
        public IHttpActionResult UpdateSection(SectionModel section)
        {
            var repo = new SectionRepository();
            var sectionReasult = repo.UpdateSection(User.Identity.Name, section);
            return Ok(Mapper.Map<Section, SectionModel>(sectionReasult));
        }



        [HttpPost]
        public IHttpActionResult Create(SectionModel section)
        {
            var repo = new SectionRepository();
            var sectionReasult = repo.CreateSection(User.Identity.Name, section);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int sectionId)
        {
            var repo = new SectionRepository();
            repo.DeleteSectionById(User.Identity.Name, sectionId);
            return Ok();
        }
  
        [HttpGet]
        public IHttpActionResult GetWorkItemList(int sectionId)
        {
            var repo = new SectionRepository();
            var reasult = repo.GetSectionWorkItemList(User.Identity.Name, sectionId).Select(Mapper.Map<WorkItem, WorkItemModel>).ToList(); ;
            return Ok(reasult);
        }
   
    }
}
