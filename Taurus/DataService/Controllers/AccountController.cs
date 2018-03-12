using DataService.Infrastructure;
using DataService.Models;
using DataService.Results;
using EF.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace DataService.Controllers
{
    [Authorize]
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private readonly ApplicationUserManager _AppUserManager = null;
        protected ApplicationUserManager AppUserManager => _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();


        private ModelFactory _modelFactory;
        protected ModelFactory TheModelFactory => _modelFactory ?? (_modelFactory = new ModelFactory(this.Request));
        private readonly IAuthRepository _authRepo;
        private readonly ICompanyRepository _companyRepo;

        private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

        public AccountController(IAuthRepository authRepo, ICompanyRepository companyRepo)
        {
            _authRepo = authRepo;
            _companyRepo = companyRepo;
        }



        [HttpGet]
        [AllowAnonymous]
        [Route("confirmemail")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            var result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return base.Content(HttpStatusCode.OK, "Your email has been confirmed", new JsonMediaTypeFormatter(), "text/plain");
            }
            //return Ok("Your email has been confirmed");
            
            else
            {
                return GetErrorResult(result);
            }
        }


        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authRepo.RegisterUser(userModel, AppUserManager);

            var errorResult = GetErrorResult(result);

            return errorResult ?? Ok();
        }

        [AllowAnonymous]
        [Route("register/company")]
        public async Task<IHttpActionResult> RegisterCompanyUser(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var companyId = _companyRepo.GetCompanyFoAdminUser(User.Identity.Name).Id;
            userModel.UserType = 1;
            var result = await _authRepo.RegisterUser(userModel, AppUserManager, companyId,true);

            var errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("register/company")]
        //public async Task<IHttpActionResult> RegisterCompanyUser(UserModel userModel)
        //{
        //    //Company user must be the type of Trade;
        //    userModel.UserType = (int)UserType.Trade;

        //    //todo generate random password
        //    if (userModel.Password == null)
        //    {
        //        userModel.Password = "123456";
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    //admin user can only register user for its company

        //    var user = await _authRepo.GetUserByUserNameAsync(User.Identity.Name);
        //    if (user == null) throw new Exception("User cannot be found");
        //    //user must be admin to create user, the check is in GetCompanyForCurrentUser


        //    var companyId = _companyRepo.GetCompanyFoAdminUser(User.Identity.Name).Id;
        //    var result = await _authRepo.RegisterUser(userModel, AppUserManager, companyId, userModel.IsContractor);

        //    var errorResult = GetErrorResult(result);

        //    return errorResult ?? Ok();
        //}


        [HttpGet]
        [Route("getcurrentuser")]
        public async Task<IHttpActionResult> GetCurrentUser()
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var user = await this._authRepo.GetUserByUserNameAsync(User.Identity.Name);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        
        [HttpGet]
        public async Task<IHttpActionResult> GetUserById(string id)
        {

            if (await _authRepo.IsUserAdminAsync(User.Identity.Name))
            {
                var user = await this._authRepo.GetUserById(id);

                if (user != null)
                {
                    return Ok(this.TheModelFactory.Create(user));
                }

            }
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            return NotFound();
        }


        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUserById(string id)
        {

            if (await _authRepo.IsUserAdminAsync(User.Identity.Name))
            {
                var user = await this._authRepo.GetUserById(id);

                if (user != null)
                {
                    if (await _authRepo.IsUserAdminAsync(user.UserName))
                    {
                        throw new Exception("Cannot delete Admin user");
                    }
                    else
                    {
                        await _authRepo.DeleteUser(id);
                        return Ok();
                    }
                }

            }
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            return NotFound();
        }

       
        [Authorize]
        [Route("updateuser")]
        public async Task<IHttpActionResult> UpdateUser(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this._authRepo.UpdateUser(User.Identity.Name, model);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }
       
        [Authorize]
        public async Task<IHttpActionResult> UpdateCompanyUser(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this._authRepo.UpdateUser(model.UserName, model);

            return !result.Succeeded ? GetErrorResult(result) : Ok();
        }

       
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [HttpGet]
        [Route("externallogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> ExternalLogin(string provider, string error = null)
        {
            string redirectUri = string.Empty;

            if (error != null)
            {
                return BadRequest(Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

            if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            {
                return BadRequest(redirectUriValidationResult);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            var user = await _authRepo.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            var hasRegistered = user != null;

            redirectUri =
                $"{redirectUri}#external_access_token={externalLogin.ExternalAccessToken}&provider={externalLogin.LoginProvider}&haslocalaccount={hasRegistered.ToString()}&external_user_name={externalLogin.UserName}";

            return Redirect(redirectUri);

        }

       
        [AllowAnonymous]
        [Route("registerexternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            ApplicationUser user = await _authRepo.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

            var hasRegistered = user != null;

            if (hasRegistered)
            {
                return BadRequest("External user is already registered");
            }

            //user = new ApplicationUser() { UserName = model.UserName };

            IdentityResult result = await _authRepo.RegisterUserWithExternalLogin(model, AppUserManager);

            //IdentityResult result = await _repo.CreateAsync(user);
            user = await _authRepo.GetUserByUserNameAsync(model.UserName);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var info = new ExternalLoginInfo()
            {
                DefaultUserName = model.UserName,
                Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
            };

            result = await _authRepo.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            //generate access token response
            var accessTokenResponse = GenerateLocalAccessTokenResponse(model.UserName);

            return Ok(accessTokenResponse);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ObtainLocalAccessToken")]
        public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
        {

            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
            {
                return BadRequest("Provider or external access token is not sent");
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            ApplicationUser user = await _authRepo.FindAsync(new UserLoginInfo(provider, verifiedAccessToken.user_id));

            bool hasRegistered = user != null;

            if (!hasRegistered)
            {
                return BadRequest("External user is not registered");
            }

            //generate access token response
            var accessTokenResponse = GenerateLocalAccessTokenResponse(user.UserName);

            return Ok(accessTokenResponse);

        }


        [AllowAnonymous]
        [HttpPost]
        [Route("resetpassword")]
        public async Task<IHttpActionResult> HandleResetPasswordRequest(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("User's email required.");
            }
            await _authRepo.SendResetPasswordCode(AppUserManager, email);
            return Ok();

        }
        [AllowAnonymous]
        [HttpPost]
        [Route("resetpassword/callback")]
        public async Task<IHttpActionResult> HandleResetPasswordCallback(ResetPasswordDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Code))
            {
                return BadRequest("");
            }
            var result = await _authRepo.ResetPassword(AppUserManager, request);
            if (result.Succeeded)
            {
                return Ok("Your password has been reset successfully");
            }
            return BadRequest(result.Errors.Aggregate((x,y)=> $"{x},{y}"));
        }

        #region Helpers

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {
            var redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            var validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out var redirectUri);

            if (!validUri)
            {
                return "redirect_uri is invalid";
            }

            var clientId = GetQueryString(Request, "client_id");

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return "client_Id is required";
            }

            var client = _authRepo.FindClient(clientId);

            //if (client == null)
            //{
            //    return string.Format("Client_id '{0}' is not registered in the system.", clientId);
            //}

            //if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            //{
            //    return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
            //}

            redirectUriOutput = redirectUri.AbsoluteUri;

            return string.Empty;

        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            return string.IsNullOrEmpty(match.Value) ? null : match.Value;
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            var verifyTokenEndPoint = "";

            switch (provider)
            {
                case "Facebook":
                    //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                    //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
                    var appToken = ConfigurationManager.AppSettings["FacebookAppToken"];
                    verifyTokenEndPoint =
                        $"https://graph.facebook.com/debug_token?input_token={accessToken}&access_token={appToken}";
                    break;
                case "Google":
                    verifyTokenEndPoint = $"https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={accessToken}";
                    break;
                default:
                    return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();

            dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

            var parsedToken = new ParsedExternalAccessToken();

            switch (provider)
            {
                case "Facebook":
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (!string.Equals(Startup.FacebookAuthOptions.AppId, parsedToken.app_id,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                    break;
                case "Google":
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    if (!string.Equals(Startup.GoogleAuthOptions.ClientId, parsedToken.app_id,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                    break;
            }

            return parsedToken;
        }

        private JObject GenerateLocalAccessTokenResponse(string userName)
        {

            var tokenExpiration = TimeSpan.FromDays(1);

            var identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);
            var userRole = "user";
            var userType = UserType.Client;

            var user = _authRepo.FindUser(userName);
            if (user == null)
            {
                throw new Exception("The user name is incorrect.");
            }
            userRole = _authRepo.GetUserRoleAsync(userName).Result;
            userType = user.UserType;
            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim(ClaimTypes.Role, userRole));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                                        new JProperty("userName",userName),
                                         new JProperty("userRole", userRole),
                                        new JProperty("userType", userType.ToString()),
                                        new JProperty("access_token", accessToken),
                                        new JProperty("token_type", "bearer"),
                                        new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                                        new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                                        new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
        );

            return tokenResponse;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; private set; }
            public string ProviderKey { get; private set; }
            public string UserName { get; private set; }
            public string ExternalAccessToken { get; private set; }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                var providerKeyClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(providerKeyClaim?.Issuer) || string.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name),
                    ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken"),
                };
            }
        }

        #endregion
    }
}

