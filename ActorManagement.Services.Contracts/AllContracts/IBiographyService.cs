using ActorManagement.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Services.Contracts.AllContracts
{
    public interface IBiographyService
    {
        Task AddBiographyAsync(Biography biography);
        Task<Biography?> GetBiographyByActorIdAsync(Guid? actorId);
        Task<Biography?> GetBiographyByIdAsync(Guid? biographyId);
        void RemoveImageFromBiography(BiographyImage existingImage);
        void UpdateBiography(Biography existBiography);
    }
}
