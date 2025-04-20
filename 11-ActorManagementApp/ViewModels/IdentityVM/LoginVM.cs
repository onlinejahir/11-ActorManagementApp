using System.ComponentModel.DataAnnotations;

namespace _11_ActorManagementApp.ViewModels.IdentityVM
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Please enter your username")]
        [Display(Name = "User Name")]         
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
