using ActorManagement.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Repositories.Contracts.AllContracts
{
    public interface IBiographyRepository : IGenericRepository<Biography>
    {
        Task<Biography?> GetBiographyByActorIdAsync(Guid? actorId);
        void RemoveImageFromBiography(BiographyImage existingImage);
    }
}
