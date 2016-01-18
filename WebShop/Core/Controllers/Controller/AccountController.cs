using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Resources;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebShop.App_GlobalResources;
using WebShop.Core.Controllers.Base;
using WebShop.Filters.Culture;
using WebShop.Filters.ModelValidate;
using WebShop.Identity.Interfaces;
using WebShop.Identity.Manager;
using WebShop.Identity.Models;
using WebShop.Infostructure.Common;
using WebShop.Infostructure.ResponseResult;
using WebShop.Infostructure.Storage.Interfaces;
using WebShop.Models.Account;
using ClaimsIdentity = System.Security.Claims.Claim;
namespace WebShop.Core.Controllers.Controller
{
    [RoutePrefix("Account")]
    [Authorize]
    public class AccountController : AccountBaseController
    {

        private IAccountService _account;

        public AccountController(ICookieConsumer storage, IAccountService account, IAuthenticationManager auth, UserManager manager,
            RoleManager role)
            : base(storage, auth, manager, role)
        {
            _account = account;
        }

        #region Login

        [Route("Login")]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? Request.UrlReferrer.AbsolutePath;
            return View();
        }

        [Route("Login"), HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ModelValidationFilter]
        public async Task<JsonResult> Login(LoginViewModel model, string returnUrl)
        {
            var result =
                await _account.LoginAsync(model.UserName, model.Password, model.RememberMe, Resource.InvalidLogin);
            if (result.Succeeded)
            {
                return Json(new { url = CheckValidReturnUrl(returnUrl) });
            }
            return new JsonResultCustom(result.Errors, HttpStatusCode.BadRequest);
        }

        #endregion

        #region Register

        [AllowAnonymous]
        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }

        [Route("Register"), HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ModelValidationFilter]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {
            User user = new User { UserName = model.UserName, Email = model.Email };
            var result = await _account.CreateUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _account.SendConfirmationTokenToEmailAsync(user.Id, Url.Action);
                return Json(Resource.RegConfirmMessage);
            }
            AddErrors(result);
            return Json(ModelState.Values.SelectMany(e => e.Errors, (m, e) => e.ErrorMessage));
        }

        [Route("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (code == null)
            {
                return View("Error");
            }
            var result = await _account.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        #endregion

        //POST: /Account/ExternalLogin
        [Route("ExternalLogin")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { returnUrl }));
        }
        //
        // GET: /Account/ExternalLoginCallback
        [Route("ExternalLoginCallback")]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await _account.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login",new {returnUrl});
            }

            var result = await _account.ExternalSignInAsync(loginInfo);
            switch (result)
            {
                case SignInStatus.Success:
                    return Redirect(CheckValidReturnUrl(returnUrl));
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }
        [Route("ExternalLoginConfirmation")]
        [AllowAnonymous]
        public ActionResult ExternalLoginConfirmation()
        {
            
            return View(new ExternalLoginConfirmationViewModel());
        }

        [Route("ExternalLoginConfirmation")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ModelValidationFilter]
        public async Task<JsonResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Json(Url.Action("Index", "Main"));
            }
            var user = new User()
            {
                Email = model.Email,
                UserName = model.UserName
            };
            var result = await _account.CreateExternalUserAsync(user);

            if (result.Succeeded)
            {
                var url = CheckValidReturnUrl(returnUrl);
                return Json(url);
            }
            AddErrors(result);
            return Json(ModelState.Values.SelectMany(e => e.Errors, (m, e) => new {field = m.Value,error=e.ErrorMessage}));
        }
        #region Log off

        [Route("LogOff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Main");
        }

        #endregion


        [HttpPost]
        public async Task<JsonResult> UserInfo(string name)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var roles = await _userManager.GetRolesAsync(user.Id);
            return Json(new { user.UserName, user.Email, roles = roles });
        }
    }
}