using System.Collections.Generic;
using WebShop.Identity.Models;

namespace WebShop.Domain.Data
{
    public class UserInfoIdentity
    {
        public bool HasPassword { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<UserExternLogin> Logins { get; set; }
  
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}