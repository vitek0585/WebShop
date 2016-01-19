using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using WebShop.Identity.Models;

namespace WebShop.Identity.Manager.CustomValidation
{
    public interface IIdentityErrros
    {
        string NameInValid { get; set; }
        string NameDuplicat { get; set; }
        string EmailInValid { get; set; }
        string EmailDuplicat { get; set; }
    }

    public class IdentityErrors : IIdentityErrros
    {
        public string NameInValid { get; set; }
        public string NameDuplicat { get; set; }
        public string EmailInValid { get; set; }
        public string EmailDuplicat { get; set; }
    }
    public class CustomUserValidation : UserValidator<User, int>
    {
        private UserManager<User, int> _manager;
        private IIdentityErrros _resource;
        public bool RequireUniqueUserName { get; set; }

        public CustomUserValidation(UserManager<User, int> manager, IIdentityErrros resource)
            : base(manager)
        {
            _resource = resource;
            _manager = manager;
        }

        public override async Task<IdentityResult> ValidateAsync(User item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("User is null");
            }
            var errors = new List<string>();
            if (RequireUniqueUserName)
            {
                await ValidateUserName(item, errors);
            }
            if (RequireUniqueEmail)
            {
                await ValidateEmail(item, errors);
            }
            if (errors.Any())
            {
                return new IdentityResult(errors);
            }
            return IdentityResult.Success;
        }

        private async Task ValidateUserName(User user, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                errors.Add(_resource.NameInValid);
            }
            else
            {
                var owner = await _manager.FindByNameAsync(user.UserName);
                if (owner != null && owner.Id != user.Id)
                {
                    errors.Add(_resource.NameDuplicat);
                }
            }
        }

        private async Task ValidateEmail(User user, List<string> errors)
        {

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                errors.Add(_resource.EmailInValid);
                return;
            }

            var owner = await _manager.FindByEmailAsync(user.Email);
            if (owner != null && owner.Id != user.Id)
            {
                errors.Add(_resource.EmailDuplicat);
            }

        }
    }
}