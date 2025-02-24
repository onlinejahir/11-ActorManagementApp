﻿using ActorManagement.Models.EntityModels;
using System.ComponentModel.DataAnnotations;

namespace _11_ActorManagementApp.ViewModels.BiographyVM
{
    public class BiographyEditVM
    {
        public Guid BiographyId { get; set; } = Guid.NewGuid();
        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;
        [StringLength(100)]
        public string DescriptionFileName { get; set; } = string.Empty;
        [Display(Name = "Upload Actor Description")]
        public IFormFile? DescriptionFile { get; set; }
        [Display(Name = "Upload Actor Images")]
        public List<IFormFile>? BioImages { get; set; } = new List<IFormFile>();
        [Required, Display(Name = "Actor")]
        public Guid ActorId { get; set; } //Foreign key to Actor(one-to-one)
        public Actor? Actor { get; set; } //Reference navigation property for foreign key

        //Relationship with BiographyImage one-to-many
        //Collection navigation property for BiographyImage
        public ICollection<BiographyImage> BiographyImages { get; set; } = new HashSet<BiographyImage>();
    }
}
