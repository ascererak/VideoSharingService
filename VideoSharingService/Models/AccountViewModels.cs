using System.ComponentModel.DataAnnotations;

namespace VideoSharingService.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.GlobalRes), ErrorMessageResourceName = "FieldRequired")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.GlobalRes), ErrorMessageResourceName = "FieldRequired")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.GlobalRes), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Resources.GlobalRes), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.GlobalRes), ErrorMessageResourceName = "FieldRequired")]
        [Display(ResourceType = typeof(Resources.GlobalRes), Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.GlobalRes), ErrorMessageResourceName = "FieldRequired")]
        [Display(ResourceType = typeof(Resources.GlobalRes), Name = "LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.GlobalRes), ErrorMessageResourceName = "FieldRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.GlobalRes), ErrorMessageResourceName = "InvalidEmail")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.GlobalRes), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(100, ErrorMessageResourceName = "ShortPasswordError", ErrorMessageResourceType = typeof(Resources.GlobalRes), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.GlobalRes), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.GlobalRes), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceName = "PwdCompareErrorMsg", ErrorMessageResourceType = typeof(Resources.GlobalRes))]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.GlobalRes), ErrorMessageResourceName = "FieldRequired")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.GlobalRes), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(100, ErrorMessageResourceName = "ShortPasswordError", ErrorMessageResourceType = typeof(Resources.GlobalRes), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.GlobalRes), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.GlobalRes), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceName = "PwdCompareErrorMsg", ErrorMessageResourceType = typeof(Resources.GlobalRes))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.GlobalRes), ErrorMessageResourceName = "FieldRequired")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
