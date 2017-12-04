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
    public class ClientsController : ApiController
    {
        private IClientRepository _clientRepo;
        private IPropertyRepository _propertyRepo;
        public ClientsController(IClientRepository clientRepo, IPropertyRepository propertyRepo)
        {
            _clientRepo = clientRepo;
            _propertyRepo = propertyRepo;
        }

        [HttpGet]
        public IHttpActionResult GetClients()
        {
            var clients = _clientRepo.GetAccessibleClientForUser(User.Identity.Name)
                .Select(Mapper.Map<Client, ClientModel>).ToList(); 
            return Ok(clients);
        }

        [HttpGet]
        public IHttpActionResult GetClient(int clientId)
        {
            var client = _clientRepo.GetClient(User.Identity.Name, clientId);
            return Ok(Mapper.Map<Client, ClientModel>(client));
        }


        [HttpDelete]
        public IHttpActionResult Delete(int clientId)
        {
            _clientRepo.DeleteClient(User.Identity.Name, clientId);
            return Ok();
        }

     

        [HttpGet]
        public IHttpActionResult GetProperties( int key)
        {
            var properties = _propertyRepo.GetPropertyForUser(User.Identity.Name)
                .Include(p => p.Address)
                .Include(p=> p.ClientProperties)
                .Where(p=>p.ClientProperties.Any(z=> z.ClientId == key));
            return Ok(properties.Select(Mapper.Map<Property, PropertyModel>).ToList());
        }

    }
}
