using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Models.EntityModels
{
    public class BiographyImage
    {
        public Guid BiographyImageId { get; set; } = Guid.NewGuid();
        [Required, StringLength(100)]
        public string ImageName { get; set; } = string.Empty;         

        //Relationship with Biography many-to-one
        [ForeignKey("Biography")]
        public Guid BiographyId { get; set; } //Foreign key to Biography
        public Biography Biography { get; set; } = null!; // Reference navigation property for foreign key
    }
}
