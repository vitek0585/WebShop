using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using WebShop.Identity.Models;

namespace WebShop.Identity.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task SendConfirmationTokenToEmailAsync(int userId, Func<string, string, object, string, string> urlHelper, string protocol = "http");
        Task<IdentityResult> ConfirmEmailAsync(int userId, string code);
        Task<IdentityResult> LoginAsync(string userName, string password, bool rememberMe = false, params string[] errors);
    }
}