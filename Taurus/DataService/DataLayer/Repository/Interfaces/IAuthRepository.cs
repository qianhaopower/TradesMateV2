
using DataService.Entities;
using DataService.Infrastructure;
using DataService.Models;

using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF.Data
{

     public  interface IAuthRepository : IBaseRepository
    {


        Task<ApplicationUser> GetUserById(string userId);
        ApplicationUser GetUserByUserName(string userName);
        Task<ApplicationUser> GetUserByUserNameAsync(string userName);
        bool isUserAdmin(string userName);

        Task<bool> IsUserAdminAsync(string userName);
        Task<bool> IsUserContractorAsync(string userName);
        Task<bool> IsUserClientAsync(string userName);
        Task<string> GetUserRoleAsync(string userName);

        Task<IdentityResult> UpdateUser(string userName, UserModel userModel);

        Task<IdentityResult> DeleteUser(string userId);

        Task<IdentityResult> RegisterUser(UserModel userModel, ApplicationUserManager appUserManager, int? companyId = null, bool isContractor = false);

        Task<IdentityResult> RegisterUserWithExternalLogin(RegisterExternalBindingModel userModel, ApplicationUserManager appUserManager, int? companyId = null);

        ApplicationUser FindUser(string userName, string password = null);

        ClientApplicaiton FindClient(string clientId);

        Task<bool> AddRefreshToken(RefreshToken token);

        Task<bool> RemoveRefreshToken(string refreshTokenId);

        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);

        Task<RefreshToken> FindRefreshToken(string refreshTokenId);

        List<RefreshToken> GetAllRefreshTokens();

        Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo);

        Task<IdentityResult> CreateAsync(ApplicationUser user);

        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);

        Task SendResetPasswordCode(ApplicationUserManager appUserManager, string email);
        Task<IdentityResult> ResetPassword(ApplicationUserManager appUserManager,ResetPasswordDTO request);

        Client GetClientForUser(string userId);

        Member GetMemberForUser(string userId);

        Client GetClientByUserName(string userName);


    }
}