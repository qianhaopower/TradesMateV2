using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using EF.Data;
using AutoMapper;
using DataService.Models;
using DataService.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Web;

namespace DataService.Controllers
{
    [Authorize]
    [RoutePrefix("api/clients")]
    public class ClientsController : ApiController
    {
        private readonly IClientRepository _clientRepo;
        private readonly IPropertyRepository _propertyRepo;
        private ApplicationUserManager _AppUserManager;

        protected ApplicationUserManager AppUserManager => _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
        public ClientsController(IClientRepository clientRepo, IPropertyRepository propertyRepo)
        {
            _clientRepo = clientRepo;
            _propertyRepo = propertyRepo;
        }


        [HttpGet]
        [Route("")]
        public IHttpActionResult Clients()
        {
            var clients = _clientRepo.GetAccessibleClientForUser(User.Identity.Name).AsEnumerable()
                .Select(Mapper.Map<Client, ClientModel>).ToList(); 
            return Ok(clients);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Client(int id)
        {
            var client = _clientRepo.GetClient(User.Identity.Name, id);
            return Ok(Mapper.Map<Client, ClientModel>(client));
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult UpdateClient (ClientModel model)
        {
            var client = _clientRepo.UpdateClient(User.Identity.Name, model);
            return Ok(Mapper.Map <Client, ClientModel>(client));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateClient(CreateNewClientRequestModel model)
        {
            var result = _clientRepo.CreateClient(User.Identity.Name, model, AppUserManager);
            return Ok(result);
        }

        [HttpPost]
        [Route("bulk")]
        public IHttpActionResult CreateClient(List<CreateNewClientBulkModel> models)
        {
            var result = _clientRepo.BulkCreateClient(User.Identity.Name, models, AppUserManager);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult RemoveClient(int id)
        {
            _clientRepo.RemoveClient(User.Identity.Name, id);
            return Ok();
        }
     

        [HttpGet]
        [Route("{id:int}/properties")]
        public IHttpActionResult Properties( int id)
        {
            var properties = _propertyRepo.GetPropertyForUser(User.Identity.Name)
                .Include(p => p.Address)
                .Include(p=> p.ClientProperties)
                .Where(p=>p.ClientProperties.Any(z=> z.ClientId == id) && !p.SystemPropertyCompanyId.HasValue);
            return Ok(properties.AsEnumerable().Select(Mapper.Map<Property, PropertyModel>).ToList());
        }
    }
}
