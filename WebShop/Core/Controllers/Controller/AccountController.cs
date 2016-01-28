﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebShop.App_GlobalResources;
using WebShop.Core.Controllers.Base;
using WebShop.Filters.Headers;
using WebShop.Filters.ModelValidate;
using WebShop.Identity.Interfaces;
using WebShop.Identity.Manager;
using WebShop.Identity.Models;
using WebShop.Infostructure.Storage.Interfaces;
using WebShop.Models.Account;

namespace WebShop.Core.Controllers.Controller
{
    [RoutePrefix("Account")]
    [Authorize]
    public class AccountController : AccountBaseController
    {

        private IAccountService _account;
        private IUnitOfWorkIdentity _unit;
        public AccountController(ICookieConsumer storage, IAccountService account, IUnitOfWorkIdentity unit)
            : base(storage)
        {
            _unit = unit;
            _account = account;
        }

        #region Login
    
        [Route("Login"), HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ModelValidationFilter]
        public async Task<JsonResult> Login(LoginViewModel model, string returnUrl)
        {
            var result =
                await _account.LoginAsync(model.UserName, model.Password, model.RememberMe, Resource.InvalidLogin.Split(','));
            if (result.Succeeded)
            {
                return Json(CheckValidReturnUrl(returnUrl));
            }
            AddErrors(result);
            return JsonResultCustom(ReturnErrorModelState(), HttpStatusCode.BadRequest);
        }

        

        #endregion

        #region Register

        [Route("Register"), HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ModelValidationFilter]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {
            User user = new User { UserName = model.UserName, Email = model.Email };
            _unit.StartTransaction();
            var result = await _account.CreateUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                try
                {
                    await _account.SendConfirmationTokenToEmailAsync(user.Id, Url.Action);
                }
                catch (Exception e)
                {
                    _unit.Rollback();
                    return JsonResultCustom("Error send message", HttpStatusCode.InternalServerError); 
                }
                _unit.Commit();

                return Json(Resource.RegConfirmMessage);
            }
            AddErrors(result);
            return JsonResultCustom(ReturnErrorModelState(),HttpStatusCode.BadRequest);
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
                default:
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
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber
            };
            var result = await _account.CreateExternalUserAsync(user);

            if (result.Succeeded)
            {
                var url = CheckValidReturnUrl(returnUrl);
                return Json(url);
            }
            AddErrors(result);
            return JsonResultCustom(ModelState.Values.SelectMany(e => e.Errors, (m, e) => e.ErrorMessage),HttpStatusCode.BadRequest);
        }
        #region Log off

        [Route("LogOff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _account.SingOut();
           
            return RedirectToAction("Index", "Main");
        }

        #endregion


        //[HttpPost]
        //public async Task<JsonResult> UserInfo(string name)
        //{
        //    var user = await _userManager.FindByNameAsync(User.Identity.Name);
        //    var roles = await _userManager.GetRolesAsync(user.Id);
        //    return Json(new { user.UserName, user.Email, roles = roles });
        //}
    }
}