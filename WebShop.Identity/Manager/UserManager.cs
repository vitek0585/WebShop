using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using WebShop.Identity.Models;
using WebShop.Infostructure.Services;

namespace WebShop.Identity.Manager
{
    public class CustomUserValidation : UserValidator<User,int>
    {
        private UserManager<User, int> _manager;
        public CustomUserValidation(UserManager<User, int> manager) : base(manager)
        {
            _manager = manager;
        }

        public async override Task<IdentityResult> ValidateAsync(User item)
        {
            IdentityResult result = await base.ValidateAsync(item);

            if (result.Succeeded)
            {
                var user = await _manager.FindByNameAsync(item.UserName);
                if (user != null)
                {
                    var errors = result.Errors.ToList();
                    errors.Add("The user name is exists My Error");
                    result = new IdentityResult(errors);
                }
            }
            return result;
        }
    }
    public class UserManager : UserManager<User, int> 
    {
        public UserManager(IUserStore<User, int> store, IDataProtectionProvider provider)
            : base(store)
        {

            UserValidator = new UserValidator<User,int>(this)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true,

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