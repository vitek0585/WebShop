using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WebShop.Identity.Context;
using WebShop.Identity.Models;

namespace WebShop.Identity.Manager
{
    public class RoleManager:RoleManager<Role,int>
    {
        public RoleManager(IRoleStore<Role, int> store) : base(store)
        {

        }

        public static RoleManager Create(IdentityFactoryOptions<RoleManager> options,IOwinContext context)
        {
            var roleStore = new RoleStore<Role,int,UserRole>(context.Get<DbContextIdentity>());
            return new RoleManager(roleStore);
        }
    }
}