using Microsoft.Owin.Security;
using WebShop.Identity.Manager;

namespace WebShop.Identity.Services
{
    public abstract class AccountGlobalService
    {
        protected RoleManager _roleManager;
        protected UserManager _userManager;
        protected IAuthenticationManager _authentication;

        protected AccountGlobalService(RoleManager roleManager, UserManager userManager, IAuthenticationManager authentication)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _authentication = authentication;
        }
    }
}