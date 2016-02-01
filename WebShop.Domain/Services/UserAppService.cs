using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using WebShop.Domain.Data;
using WebShop.Domain.Interfaces;
using WebShop.Domain.Services.Common;
using WebShop.EFModel.Model;
using WebShop.Identity.Interfaces;
using WebShop.Identity.Manager;
using WebShop.Repo.Interfaces;

namespace WebShop.Domain.Services
{
    public class UserAppService : AccountGlobalService, IUserAppService
    {
        private ISaleService _saleService;
   
        public UserAppService(RoleManager roleManager, UserManager userManager, SignInManager
            singInManager, IAuthenticationManager authentication, ISaleService saleService) :
            base(roleManager, userManager, singInManager, authentication)
        {
           
            _saleService = saleService;
        }

        public int GetUserId()
        {
            if(!_authentication.User.Identity.IsAuthenticated)
                throw new AuthenticationException("User did not authenticate");

            return _authentication.User.Identity.GetUserId<int>();
        }
        public async Task<UserInfoIdentity> GetUserInfoAsync(int? id)
        {
            if(!id.HasValue)
            id = GetUserId();

            var userInfo = new UserInfoIdentity();
            var user = await _userManager.FindByIdAsync(id.Value);
            
            userInfo.HasPassword = user.PasswordHash != null;
            userInfo.PhoneNumber = user.PhoneNumber;
            userInfo.Logins = user.Logins;
            userInfo.UserName = user.UserName;
            userInfo.Email = user.Email;

            return userInfo;
        }
        public IEnumerable<TResult> UserSales<TResult>(int? id,int page, int totalPerPage,string currentCurrency, string lang)
        {
            if (!id.HasValue)
                id = GetUserId();

            return _saleService.SaleByPage<TResult>(id.Value, page, totalPerPage, currentCurrency, lang);
        }
        
        #region additional methods

        #endregion

    }
}