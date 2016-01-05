using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using WebShop.Identity.Context;
using WebShop.Identity.Manager;
using WebShop.Identity.Models;

namespace WebShop.Identity.StartUp
{
    public class StartUpApp
    {
        public void ConfigureAuth(IAppBuilder app, string pathToLogin)
            
        {
            
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString(pathToLogin),
                Provider = new CookieAuthenticationProvider()
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<UserManager, User, int>(
                    TimeSpan.FromMinutes(30),
                    (m, u) => u.GenerateUserIdentityAsync(m),
                    c => c.GetUserId<int>())
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ApplicationCookie);

            
        }
    }
}