using System.Web;
using System.Web.Mvc;
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

        #region Custom action result
        // Used for XSRF protection when adding external logins
        protected const string XsrfKey = "XsrfId";
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion

    }
}