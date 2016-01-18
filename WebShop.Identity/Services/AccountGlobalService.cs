﻿using Microsoft.Owin.Security;
using WebShop.Identity.Manager;

namespace WebShop.Identity.Services
{
    public abstract class AccountGlobalService
    {
        protected RoleManager _roleManager;
        protected UserManager _userManager;
        protected IAuthenticationManager _authentication;
        protected SignInManager _singInManager;
        protected AccountGlobalService(RoleManager roleManager, UserManager userManager,SignInManager singInManager,
            IAuthenticationManager authentication)
        {
            _singInManager = singInManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _authentication = authentication;
        }
    }
}