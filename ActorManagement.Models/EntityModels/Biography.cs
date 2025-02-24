using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ActorManagement.Models.EntityModels
{
    public class Biography
    {
        public Guid BiographyId { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string DescriptionFileName { get; set; } = string.Empty;

        //Relationship with Actor one-to-one
        [ForeignKey("Actor")]
        public Guid ActorId { get; set; } //Foreign key to Actor(one-to-one)
        [JsonIgnore] // Prevents serialization loop
        public Actor Actor { get; set; } = null!; //Reference navigation property for foreign key

        //Relationship with BiographyImage one-to-many
        //Collection navigation property for BiographyImage
        public ICollection<BiographyImage> BiographyImages { get; set; } = new HashSet<BiographyImage>();
    }
}
