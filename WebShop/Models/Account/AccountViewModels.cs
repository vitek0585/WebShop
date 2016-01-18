using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebShop.App_GlobalResources;

namespace WebShop.Models.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessageResourceName = "NameInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^[A-Za-z]+\w{2,20}",
            ErrorMessageResourceName = "NameInValid", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "EmailInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$",
            ErrorMessageResourceName = "EmailInValid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {

        [Required]
        [Display(Name = "User Name")]
        [RegularExpression(@"^[A-Za-z]+\w{2,20}",
            ErrorMessage = "The user name should be minimum 3 and maximum 20 characters, and first character must be a letter")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceName = "NameInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^[A-Za-z]+\w{2,20}",
            ErrorMessageResourceName = "NameInValid", ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "EmailInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$",
            ErrorMessageResourceName = "EmailInValid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "PaswdInValid", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^(?=[a-zA-Z])(?=[a-zA-Z0-9]*)(?!.*\s).{6,20}$",
           ErrorMessageResourceName = "PaswdInValid", ErrorMessageResourceType = typeof(Resource))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "ConfirmPswdInValid", ErrorMessageResourceType = typeof(Resource))]
        [Compare("Password", ErrorMessageResourceName = "ConfirmPswdInValid", ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
