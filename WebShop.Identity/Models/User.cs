using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebShop.Identity.Manager;

namespace WebShop.Identity.Models
{
    public class User:IdentityUser<int,UserExternLogin,UserRole,Claim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager userManager)
        {
            var userIdentity = await userManager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }
    }
}