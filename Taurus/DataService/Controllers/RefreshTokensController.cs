
using DataService.Infrastructure;
using EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DataService.Controllers
{
    [RoutePrefix("api/RefreshTokens")]
    public class RefreshTokensController : ApiController
    {
        private IAuthRepository _authRepo;
     
        public RefreshTokensController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [Authorize(Users="Admin")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(_authRepo.GetAllRefreshTokens());
        }

        //[Authorize(Users = "Admin")]
        [AllowAnonymous]
        [Route("")]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            var result = await _authRepo.RemoveRefreshToken(tokenId);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Token Id does not exist");
            
        }

    }
}
