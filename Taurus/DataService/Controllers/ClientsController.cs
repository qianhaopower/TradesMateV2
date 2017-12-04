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
        [HttpGet]
        public IHttpActionResult GetClients()
        {
            var repo = new ClientRepository();
            var clients =  repo.GetAccessibleClientForUser(User.Identity.Name)
                .Select(Mapper.Map<Client, ClientModel>).ToList(); 
            return Ok(clients);
        }

        [HttpGet]
        public IHttpActionResult GetClient(int clientId)
        {
            var repo = new ClientRepository();
            var client = repo.GetClient(User.Identity.Name, clientId);
            return Ok(Mapper.Map<Client, ClientModel>(client));
        }


        [HttpDelete]
        public IHttpActionResult Delete(int clientId)
        {
            var repo = new ClientRepository();
            repo.DeleteClient(User.Identity.Name, clientId);
            return Ok();
        }

     

        [HttpGet]
        public IHttpActionResult GetProperties( int key)
        {
            var repo = new PropertyRepository();

            var properties = repo.GetPropertyForUser(User.Identity.Name)
                .Include(p => p.Address)
                .Include(p=> p.ClientProperties)
                .Where(p=>p.ClientProperties.Any(z=> z.ClientId == key));
            return Ok(properties.Select(Mapper.Map<Property, PropertyModel>).ToList());
        }

    }
}
