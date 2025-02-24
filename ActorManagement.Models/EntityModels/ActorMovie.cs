using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Models.EntityModels
{
    //Junction table entity for many-to-many relationship
    public class ActorMovie
    {
        public Guid ActorId { get; set; }
        public Actor Actor { get; set; } = null!; //Reference navigation property

        public Guid MovieId { get; set; }
        public Movie Movie { get; set; } = null!; //Reference navigation property
    }
}
