using WebShop.Identity.Context;
using WebShop.Identity.Interfaces;
using WebShop.Identity.Models;

namespace WebShop.Identity.Repository
{
    public class UserRepository : IdentityGlobalRepository<User>, IUserRepository
    {
        public UserRepository(DbContextIdentity context)
            : base(context)
        {

        }
    }
}