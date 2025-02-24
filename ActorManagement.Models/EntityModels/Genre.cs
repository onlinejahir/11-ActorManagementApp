using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Models.EntityModels
{
    public class Genre
    {
        public Guid GenreId { get; set; } = Guid.NewGuid();
        [Required, StringLength(100)]
        public string GenreName { get; set; } = string.Empty;
        [Required, StringLength(150)]
        public string Description { get; set; } = string.Empty;
        [Required, DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        [Required]
        public bool IsActive { get; set; }

        //Relationship with Movie one-to-many
        public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>(); //Collection navigation property
    }
}
