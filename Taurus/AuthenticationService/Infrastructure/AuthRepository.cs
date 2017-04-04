using AuthenticationService.Entities;
using AuthenticationService.Models;
using EF.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace AuthenticationService.Infrastructure
{

    public class AuthRepository : IDisposable
    {
        private AuthContext _ctx;
        private EFDbContext _applicationContext;

        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _applicationContext = new EFDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AuthContext()));
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

        public async Task<ApplicationUser> GetUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception("User name cannot by empty");
            }
            var user = await _userManager.FindByNameAsync(userName);
            return user;
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

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                  LastName = userModel.LastName,
                  UserType = userModel.UserType,
                  JoinDate = DateTime.Now,

             };
            //Check if the user is client or trademan.
            if (userModel.UserType == (int)UserType.Client)
            {
                var result = await _userManager.CreateAsync(user, userModel.Password);

                return result;

            }
            else if(userModel.UserType == (int)UserType.Trade)
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
                        result = _userManager.AddToRole(user.Id, "Admin");

                    return result;
                }

            }
            else
            {
                throw new Exception("User type cannot be recognized");
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

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}