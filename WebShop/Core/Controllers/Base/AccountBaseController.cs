using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using WebShop.Identity.Manager;
using WebShop.Infostructure.Storage.Interfaces;
using WebShop.Repo.Interfaces;

namespace WebShop.Core.Controllers.Base
{
    public abstract class AccountBaseController : ShopBaseController
    {
        protected RoleManager _roleManager;
        protected UserManager _userManager;
        protected IAuthenticationManager _authentication;
        public AccountBaseController(ICookieConsumer storage, IAuthenticationManager auth, UserManager manager, RoleManager role)
            : base(storage)
        {
            _roleManager = role;
            _userManager = manager;
            _authentication = auth;

        }
        #region Helpers method
        

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion
    }
}