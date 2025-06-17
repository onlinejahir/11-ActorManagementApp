using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace _11_ActorManagementApp.ViewModels.IdentityVM
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Please enter your first name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter your last name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter your date of birth")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Please enter user name")]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter your email")]
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password doesn't match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
