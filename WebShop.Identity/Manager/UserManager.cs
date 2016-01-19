using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using WebShop.Identity.Manager.CustomValidation;
using WebShop.Identity.Models;
using WebShop.Infostructure.Services;

namespace WebShop.Identity.Manager
{
    
    public class UserManager : UserManager<User, int>
    {
        public UserManager(IUserStore<User, int> store, IDataProtectionProvider provider, IIdentityErrros identity)
            : base(store)
        {

            //UserValidator = new UserValidator<User, int>(this)
            //{
            //    AllowOnlyAlphanumericUserNames = true,
            //    RequireUniqueEmail = true,

            //};
            UserValidator = new CustomUserValidation(this, identity)
            {
                RequireUniqueEmail = true,
                RequireUniqueUserName = true
            };
            PasswordValidator = new PasswordValidator()
            {
                RequireDigit = false,
                RequiredLength = 5,
                RequireLowercase = false,
                RequireUppercase = false,
                RequireNonLetterOrDigit = false
            };

            EmailService = new MailService();

            if (provider != null)
            {
                UserTokenProvider =
                    new DataProtectorTokenProvider<User, int>(provider.Create("ASP.NET Identity"))
                    {
                        TokenLifespan = TimeSpan.FromHours(6)
                    };
            }
        }




    }


}