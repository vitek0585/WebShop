using System.Threading.Tasks;
using WebShop.Identity.Data;

namespace WebShop.Identity.Interfaces
{
    public interface IUserIdentityService
    {
        Task<UserInfoIdentity> GetUserInfo(int id);
        int GetUserId();
    }
}