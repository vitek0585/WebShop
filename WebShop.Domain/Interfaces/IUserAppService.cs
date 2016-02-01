using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Domain.Data;

namespace WebShop.Domain.Interfaces
{
    public interface IUserAppService
    {
        Task<UserInfoIdentity> GetUserInfoAsync(int? id);
        IEnumerable<TResult> UserSales<TResult>(int? id, int page, int totalPerPage, string currentCurrency, string lang);
        int GetUserId();
    }
}