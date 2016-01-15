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
    [TypeOfCulture]
    public class AccountController : AccountBaseController
    {
        // Used for XSRF protection when adding external logins
        protected const string XsrfKey = "XsrfId";
        private IAccountService _account;

        public AccountController(ICookieConsumer storage, IAccountService account, IAuthenticationManager auth, UserManager manager,
            RoleManager role)
            : base(storage, auth, manager, role)
        {
            _account = account;
        }
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
            var result = await _account.LoginAsync(model.UserName, model.Password, model.RememberMe, Resource.InvalidLogin);
            if (result.Succeeded)
            {
                return Json(new { url = CheckValidReturnUrl(returnUrl) });
            }
            return new JsonResultCustom(result.Errors, HttpStatusCode.BadRequest);
        }

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
        [TypeOfCulture]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {
            var result = IdentityResult.Success;
           ICookieConsumer storage = DependencyResolver.Current.GetService<ICookieConsumer>();

            var language = storage.GetValueStorage(HttpContext, ValuesApp.Language) ??
                       ValuesApp.LanguageDefault;

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);

            var currency = storage.GetValueStorage(HttpContext, ValuesApp.Currency) ??
                       ValuesApp.CurrencyDefault;

            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol = ValuesApp.GetCurrencySymbol(language,currency);
            Thread.CurrentThread.CurrentUICulture.NumberFormat.CurrencySymbol = ValuesApp.GetCurrencySymbol(language, currency);
            //User user = new User { UserName = model.UserName, Email = model.Email };
            //var result = await _account.CreateUserAsync(user, model.Password);
            if (result.Succeeded)
            {
              
               // await _account.SendConfirmationTokenToEmailAsync(user.Id, Url.Action);
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
        [Route("LogOff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Main");
        }

        [HttpPost]
        public async Task<JsonResult> UserInfo(string name)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var roles = await _userManager.GetRolesAsync(user.Id);
            return Json(new { user.UserName, user.Email, roles = roles });
        }
    }
}