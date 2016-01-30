using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using WebShop.Identity.Data;
using WebShop.Identity.Interfaces;
using WebShop.Identity.Manager;
using WebShop.Identity.Models;

namespace WebShop.Identity.Services
{
    public class UserIdentityService : AccountGlobalService, IUserIdentityService
    {
        public UserIdentityService(RoleManager roleManager, UserManager userManager, SignInManager
            singInManager, IAuthenticationManager authentication) :
            base(roleManager, userManager, singInManager, authentication)
        {
        }

        public int GetUserId()
        {
            if(!_authentication.User.Identity.IsAuthenticated)
                throw new AuthenticationException("User did not authenticate");

            return _authentication.User.Identity.GetUserId<int>();
        }
        public async Task<UserInfoIdentity> GetUserInfo(int id)
        {
            var userInfo = new UserInfoIdentity();
            var user = await _userManager.FindByIdAsync(id);
            
            userInfo.HasPassword = user.PasswordHash != null;
            userInfo.PhoneNumber = user.PhoneNumber;
            userInfo.Logins = user.Logins;
            userInfo.UserName = user.UserName;
           
            return userInfo;
        }
        #region additional methods

        #endregion

    }
}