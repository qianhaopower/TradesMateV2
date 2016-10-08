
using DataService.Entities;
using DataService.Infrastructure;
using DataService.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace EF.Data
{

    public class AuthRepository : IDisposable
    {
        private EFDbContext _ctx;
        private EFDbContext _applicationContext;

        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AuthRepository()
        {
            _ctx = new EFDbContext();
            _applicationContext = new EFDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new EFDbContext()));
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("UserId cannot by empty");
            }
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public  List<ApplicationUser> GetUserByCompanyId(int companyId)
        {

            var usersByCompanyId =  _ctx.Users.Where(user => user.CompanyId == companyId).ToList();
            return usersByCompanyId;
        }

        public  bool IsUserInRole(string userId,string roleName)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("UserId cannot by empty");
            }
            var isInRole =  _userManager.IsInRole(userId, roleName);

            return isInRole;
        }

        public async Task<ApplicationUser> GetUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception("User name cannot by empty");
            }
            var user = await _userManager.FindByNameAsync(userName);
            return user;
        }

        public async Task<bool> isUserAdmin(string userName)
        {
            var user = await this.GetUserByUserName(userName);
            if (user != null)
            {

                if (this.IsUserInRole(user.Id, "Admin"))
                {
                    return true;
                }
                return false;
            }
            return false;
        }


        public async Task<IdentityResult> UpdateUser(string userName, UserModel userModel)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception("User name cannot by empty");
            }
            var user = await _userManager.FindByNameAsync(userName);

            //put all the updatable field here. Interestingly we can update user name. 
            user.UserName = userModel.UserName;
            user.Email = userModel.Email;
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
           
          
            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<IdentityResult> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User name cannot by empty");
            }
            var user = await _userManager.FindByIdAsync(userId);     
            var result = await _userManager.DeleteAsync(user);
            return result;
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel, ApplicationUserManager appUserManager, int? companyId = null)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                UserType = userModel.UserType,
                JoinDate = DateTime.Now,
                CompanyId = companyId.HasValue ? companyId.Value : 0,
                Email = userModel.Email,

            };
            //Check if the user is client or trademan.
            if (userModel.UserType == (int)UserType.Client
                ||( userModel.UserType == (int)UserType.Trade && companyId.HasValue))
            {
                var result = await _userManager.CreateAsync(user, userModel.Password);

                //need create client entity here
                Client newClient = new Client
                {
                    FirstName = user.FirstName,
                    SurName = user.LastName,
                    Email = user.Email,
                    UserId =  user.Id,
                  
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                _ctx.Entry(newClient).State = EntityState.Added;
                _ctx.Clients.Add(newClient);
                _ctx.SaveChanges();

                user.Client = newClient;
                 result = await _userManager.UpdateAsync(user);

                // no need to wait for this let it send. 
                await SendConfirmEmail(result, appUserManager, user);
                return result;

            }
            else if(userModel.UserType == (int)UserType.Trade && !companyId.HasValue)
            {
                // a company name should be provided, 

                if (string.IsNullOrEmpty(userModel.CompanyName))
                {
                    throw new Exception("Please provide a company name when registering as tradespeople");
                }
                else
                {

                    //we create a new company and set the user to be the admin of the company
                    var company = new Company()
                    {
                        Name = userModel.CompanyName,
                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Description = string.Format("A company created by user {0} {1}", userModel.FirstName, userModel.LastName)
                    };

                    _applicationContext.Entry(company).State = EntityState.Added;
                    _applicationContext.SaveChanges();

                    //set the user company id to the user.
                    user.CompanyId  = _applicationContext.Companies.Where(p => p.Name == userModel.CompanyName ).First().Id;

                    var result = await _userManager.CreateAsync(user, userModel.Password);

                    //add the user to the Admin role
                    if (result.Succeeded)
                    {
                        result = _userManager.AddToRole(user.Id, "Admin");
                    }
                    await SendConfirmEmail(result, appUserManager, user);
                    return result;
                }

            }
            else
            {
                throw new Exception("User type cannot be recognized");
            }



         
        }



        private async Task SendConfirmEmail(IdentityResult result, ApplicationUserManager appUserManager , ApplicationUser user)
        {

            if (result.Succeeded)
            {
                string code = await appUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                code = HttpUtility.UrlEncode(code);
                var ip = GetLocalIPAddress();
                // var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));
                var callbackUrl = string.Format("{2}/DataService/api/account/ConfirmEmail?userId={0}&code={1}", user.Id, code, ip);


                await appUserManager.SendEmailAsync(user.Id,
                                                        "Confirm your account",
                                                        "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a> <br/> <br/>"
                                                        + "If the link cannot be open, please copy and past the following url to you browser.Thanks. <br/> <br/>"
                                                        + callbackUrl);
            }
        }

        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            ApplicationUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public ClientApplicaiton FindClient(string clientId)
        {
            var client = _ctx.ClientApplications.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

           var existingToken = _ctx.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

           if (existingToken != null)
           {
             var result = await RemoveRefreshToken(existingToken);
           }
          
            _ctx.RefreshTokens.Add(token);

            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
           var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

           if (refreshToken != null) {
               _ctx.RefreshTokens.Remove(refreshToken);
               return await _ctx.SaveChangesAsync() > 0;
           }

           return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _ctx.RefreshTokens.Remove(refreshToken);
             return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
             return  _ctx.RefreshTokens.ToList();
        }

        public async Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo)
        {
            ApplicationUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}