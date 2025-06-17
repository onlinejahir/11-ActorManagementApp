using System.ComponentModel.DataAnnotations;

namespace _11_ActorManagementApp.ViewModels.IdentityVM
{
    public class EmailViewModel
    {
        [Required(ErrorMessage = "Please enter your name")]
        [Display(Name = "Your Name")]
        public string PersonName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter your email address")]
        [Display(Name = "Your Email")]
        [EmailAddress]
        public string PersonEmail { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter your subject")]
        public string Subject { get; set; } = string.Empty;
        [Required]
        public string Message { get; set; } = string.Empty;
    }
}
