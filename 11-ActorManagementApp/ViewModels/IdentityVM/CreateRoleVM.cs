using System.ComponentModel.DataAnnotations;

namespace _11_ActorManagementApp.ViewModels.IdentityVM
{
    public class CreateRoleVM
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;
    }
}
