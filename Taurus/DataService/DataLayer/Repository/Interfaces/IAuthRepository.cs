
using DataService.Entities;
using DataService.Infrastructure;
using DataService.Models;

using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF.Data
{

     public  interface IAuthRepository 
    {


        Task<ApplicationUser> GetUserById(string userId);



        ApplicationUser GetUserByUserName(string userName);



        Task<ApplicationUser> GetUserByUserNameAsync(string userName);

        bool isUserAdmin(string userName);

        Task<bool> isUserAdminAsync(string userName);


        Task<IdentityResult> UpdateUser(string userName, UserModel userModel);

        Task<IdentityResult> DeleteUser(string userId);

        Task<IdentityResult> RegisterUser(UserModel userModel, ApplicationUserManager appUserManager, int? companyId = null, bool isContractor = false);

        //external login with no password required
        Task<IdentityResult> RegisterUserWithExternalLogin(RegisterExternalBindingModel userModel, ApplicationUserManager appUserManager, int? companyId = null);



        ApplicationUser FindUser(string userName, string password);

        ClientApplicaiton FindClient(string clientId);

        Task<bool> AddRefreshToken(RefreshToken token);

        Task<bool> RemoveRefreshToken(string refreshTokenId);

        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);

        Task<RefreshToken> FindRefreshToken(string refreshTokenId);

        List<RefreshToken> GetAllRefreshTokens();

        Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo);

        Task<IdentityResult> CreateAsync(ApplicationUser user);

        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);



    }
}