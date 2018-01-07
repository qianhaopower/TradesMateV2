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
    [RoutePrefix("api/properties")]
    public class PropertiesController : ApiController
    {
        private IAuthRepository _authRepo;
        private readonly IPropertyRepository _propRepo;
        public PropertiesController(IAuthRepository authRepo, IPropertyRepository propRepo)
        {
            _authRepo = authRepo;
            _propRepo = propRepo;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetProperties()
        {
            var repo = _propRepo;
            var properties = repo.GetPropertyForUser(User.Identity.Name).Include(p => p.Address);
            return Ok(properties.AsEnumerable().Select(Mapper.Map<Property, PropertyModel>).ToList());
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult UpdatePropertyForClient(PropertyModel model)
        {
            var property = _propRepo.UpdatePropertyForClient(User.Identity.Name, model);
            return Ok(property);
        }


        [HttpPost]
        [Route("")]
        public IHttpActionResult CreatePropertyForClient(PropertyModel model)
        {
            var property = _propRepo.CreatePropertyForClient(User.Identity.Name, model);
            return Ok();//need use created and pass in the url for the new resource
        }
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetProperty(int id)
        {
            var repo = _propRepo;
            var property = repo.GetPropertyForUser(User.Identity.Name).Include(p => p.Address).FirstOrDefault(p => p.Id == id);
            var result = Mapper.Map<Property, PropertyModel>(property);

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var repo = _propRepo;
            repo.DeleteProperty(User.Identity.Name, id);
            return Ok();

        }

        [HttpGet]
        [Route("{id:int}/report")]
        public IHttpActionResult GetPropertyReportItems(int id)
        {
            var repo = _propRepo;
            var hasPermission = repo.GetPropertyForUser(User.Identity.Name).Any(p=> p.Id == id);
            if (!hasPermission)
                return  Unauthorized();
            var result = repo.GetPropertyReportData(id, User.Identity.Name);
            return    Ok(result);

        }


        [HttpGet]
        [Route("{memberId:int}/allocation")]
        public IHttpActionResult GetMemberAllocation(int memberId)
        {
            var memberList = _propRepo.GetMemberAllocation(User.Identity.Name, memberId).ToList();
            return (Ok(memberList));
        }


        [HttpPost]
        [Route("{id:int}/allocation/{memberId:int}/{allocate:bool}")]
        public IHttpActionResult UpdateMemberAllocation(int id, int memberId, bool allocate)
        {
            var allcation = _propRepo.UpdateMemberAllocation(User.Identity.Name, id, memberId, allocate);
            return (Ok(allcation));
        }
      
        [HttpGet]
        [Route("{id:int}/company")]
        public IHttpActionResult GetCompanies(int id)
        {
            var repo = _propRepo;
            var companyModels = repo.GetCompanyForProperty(id).Select(Mapper.Map<Company, CompanyModel>).ToList();
            //no need for the credit card field
            companyModels.ForEach(p => p.CreditCard = null);
            return Ok(companyModels);
        }

     

        [HttpGet]
        [Route("{id:int}/section")]
        public IHttpActionResult GetSectionList(int id)
        {
            var repo = _propRepo;
            var sections = repo.GetPropertySectionList(User.Identity.Name, id);
            return Ok(sections.Select(Mapper.Map<Section, SectionModel>));
        }

    }

}
