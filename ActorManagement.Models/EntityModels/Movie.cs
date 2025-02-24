using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Models.EntityModels
{
    public class Movie
    {
        public Guid MovieId { get; set; } = Guid.NewGuid();
        [Required, StringLength(100)]
        public string MovieName { get; set; } = string.Empty;
        [Required, StringLength(250)]
        public string Description { get; set; } = string.Empty;
        [Required, DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        [Required, StringLength(100)]
        public string ThumbnailName { get; set; } = string.Empty;

        //Relationship with Genre many-to-one
        [ForeignKey("Genre")]
        public Guid GenreId { get; set; } //Foreign key to Genre
        public Genre Genre { get; set; } = null!; //Reference navigation property for foreign key

        //Relationship with Actor many-to-many
        public ICollection<ActorMovie> ActorMovies { get; set; } = new HashSet<ActorMovie>(); //Collection navigation property
    }
}
