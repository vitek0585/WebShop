using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using WebShop.Identity.Interfaces;
using WebShop.Identity.Manager;
using WebShop.Identity.Models;

namespace WebShop.Identity.Services
{
    public class AccountService:AccountGlobalService,IAccountService
    {
        public AccountService(RoleManager roleManager, UserManager userManager, 
            IAuthenticationManager authentication) : base(roleManager, userManager, authentication)
        {
        }

        public Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public async Task SendConfirmationTokenToEmailAsync(int userId, Func<string, string, object, string, string> urlHelper, string protocol = "http")
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = urlHelper("ConfirmEmail", "Account", new { userId = userId, code = code },protocol);

            await _userManager.SendEmailAsync(userId, "Confirm your account",
                "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>  in the my shop is Vitek prodaction");
        }
    }
}