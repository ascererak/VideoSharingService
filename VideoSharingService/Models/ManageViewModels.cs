using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace VideoSharingService.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.GlobalRes), ErrorMessageResourceName = "FieldRequired")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.GlobalRes), Name = "CurrentPassword")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.GlobalRes), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(100, ErrorMessageResourceName = "ShortPasswordError", ErrorMessageResourceType = typeof(Resources.GlobalRes), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.GlobalRes), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.GlobalRes), Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessageResourceName = "PwdCompareErrorMsg", ErrorMessageResourceType = typeof(Resources.GlobalRes))]
        public string ConfirmPassword { get; set; }
    }
}