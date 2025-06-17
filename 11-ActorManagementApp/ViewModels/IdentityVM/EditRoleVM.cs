using System.ComponentModel.DataAnnotations;

namespace _11_ActorManagementApp.ViewModels.IdentityVM
{
    public class EditRoleVM
    {
        public string Id { get; set; } = string.Empty;
        [Required]
        public string? RoleName { get; set; } = string.Empty;
        public List<string> Users { get; set; } = new List<string>();
    }
}
