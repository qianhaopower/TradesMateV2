using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using EF.Data;
using AutoMapper;
using DataService.Models;

namespace DataService.Controllers
{
    [Authorize]
    public class PropertiesWebApiController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetProperties()
        {
            var repo = new PropertyRepository();
            var properties = repo.GetPropertyForUser(User.Identity.Name).Include(p => p.Address);
            return Ok(properties.Select(Mapper.Map<Property, PropertyModel>).ToList());
        }

        [HttpPost]
        public IHttpActionResult CreatePropertyForClient(PropertyModel model)
        {
            var property = new PropertyRepository().CreatePropertyForClient(User.Identity.Name, model);
            return Ok();
        }


        [HttpGet]
        public IHttpActionResult GetPropertyReportItems(int propertyId)
        {
            var repo = new PropertyRepository();
            var hasPermission = repo.GetPropertyForUser(User.Identity.Name).Any(p=> p.Id == propertyId);
            if (!hasPermission)
                return  Unauthorized();
            var result = repo.GetPropertyReportData(propertyId, User.Identity.Name);
            return    Ok(result);

        }


        [HttpGet]
        public IHttpActionResult GetMemberAllocation(int memberId)
        {
            var memberList = new PropertyRepository().GetMemberAllocation(User.Identity.Name, memberId).ToList();
            return (Ok(memberList));
        }


        [HttpPost]
        public IHttpActionResult UpdateMemberAllocation(int propertyId, int memberId, bool allocate)
        {
            var allcation = new PropertyRepository().UpdateMemberAllocation(User.Identity.Name, propertyId, memberId, allocate);
            return (Ok(allcation));
        }
      
        [HttpGet]
        public IHttpActionResult GetCompanies(int propertyId)
        {
            var repo = new PropertyRepository();
            var companyModels = repo.GetCompanyForProperty(propertyId).Select(Mapper.Map<Company, CompanyModel>).ToList();
            //no need for the credit card field
            companyModels.ForEach(p => p.CreditCard = null);
            return Ok(companyModels);
        }

        [HttpGet]
        public IHttpActionResult GetProperty(int key)
        {
            var repo = new PropertyRepository();
            var property = repo.GetPropertyForUser(User.Identity.Name).Include(p => p.Address).FirstOrDefault(p => p.Id == key);
            var result = Mapper.Map<Property, PropertyModel>(property);
          
            return Ok(result);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int key)
        {
            var repo = new PropertyRepository();
            repo.DeleteProperty(User.Identity.Name, key);
            return Ok();
          
        }
        [HttpGet]
        public IHttpActionResult GetSectionList(int key)
        {
            var repo = new PropertyRepository();
            var sections = repo.GetPropertySectionList(User.Identity.Name, key);
            return Ok(sections.Select(Mapper.Map<Section, SectionModel>));
        }

    }

}
