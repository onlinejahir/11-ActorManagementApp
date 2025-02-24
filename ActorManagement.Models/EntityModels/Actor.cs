using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ActorManagement.Models.EntityModels
{
    public class Actor
    {
        public Guid ActorId { get; set; } = Guid.NewGuid();
        [Required, StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        [Required, DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required, StringLength(50)]
        public string Gender { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string ImageName { get; set; } = string.Empty;

        //Relationship with Biography one-to-one
        [JsonIgnore] // Prevents serialization loop
        public Biography Biography { get; set; } = null!; //Navigation property for Biography

        //Relationship with Movie many-to-many
        public ICollection<ActorMovie> ActorMovies { get; set; } = new HashSet<ActorMovie>(); //Collection navigation property
    }
}
