using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using WebShop.Core.Controllers.Base;
using WebShop.Filters.ModelValidate;
using WebShop.Identity.Manager;
using WebShop.Identity.Models;
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
        // Used for XSRF protection when adding external logins
        protected const string XsrfKey = "XsrfId";

        public AccountController(ICookieConsumer storage, IAuthenticationManager auth, UserManager manager,
            RoleManager role) : base(storage, auth, manager, role)
        {
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? Request.UrlReferrer.AbsolutePath;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ModelValidationFilter]
        public async Task<JsonResult> Login(LoginViewModel model, string returnUrl)
        {
            var user = await _userManager.FindAsync(model.UserName, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login or password");
                return new JsonResultCustom(ModelState.Select(s => s.Value.Errors), HttpStatusCode.BadRequest);
            }
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Email not confirmed.");
                return new JsonResultCustom(ModelState.Select(s => s.Value.Errors), HttpStatusCode.BadRequest);
            }
            var ident = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            if (user.UserName == "vitek")
            {
                ident.AddClaim(new ClaimsIdentity(ClaimTypes.Role, "Admin"));
            }
            _authentication.SignIn(new AuthenticationProperties() { IsPersistent = model.RememberMe }, ident);

            return Json(new { url = CheckValidReturnUrl(returnUrl) });
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ModelValidationFilter]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {
            User user = new User { UserName = model.UserName, Email = model.Email };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                    Request.Url.Scheme);

                await _userManager.SendEmailAsync(user.Id, "Confirm your account",
                    "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>  in the my shop is Vitek prodaction");

                return Json(new
                {
                    message = "To your e-mail was sent message to confirm your registration"
                });
            }

            AddErrors(result);
            return new JsonResultCustom(ModelState.Values.Select(e => e.Errors), HttpStatusCode.BadRequest);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (code == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
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