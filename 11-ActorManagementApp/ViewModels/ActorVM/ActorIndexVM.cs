using ActorManagement.Models.EntityModels;
using System.ComponentModel.DataAnnotations;

namespace _11_ActorManagementApp.ViewModels.ActorVM
{
    public class ActorIndexVM
    {
        public Guid ActorId { get; set; } = Guid.NewGuid();
        [Required, StringLength(100), Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        [Required, StringLength(100), Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;
        [Required, DataType(DataType.Date), Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required, StringLength(50)]
        public string Gender { get; set; } = string.Empty;
        [Required, StringLength(100), EmailAddress, Display(Name = "Email ID")]
        public string Email { get; set; } = string.Empty;
        [Required, Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please upload actor image."), Display(Name = "Upload Image")]
        //[FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only image file (jpg, jpeg, png) are allowed.")]
        public IFormFile ImageFile { get; set; } = null!;

        //Relationship with Biography one-to-one        
        public Biography? Biography { get; set; } //Navigation property for Biography
    }
}
