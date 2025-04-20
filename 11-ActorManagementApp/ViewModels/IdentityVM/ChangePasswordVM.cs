using System.ComponentModel.DataAnnotations;

namespace _11_ActorManagementApp.ViewModels.IdentityVM
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "Please enter your current password")]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter your new password")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please confirm your new password")]
        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New password doesn't match")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
