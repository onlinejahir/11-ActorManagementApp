using ActorManagement.Models.EntityModels;
using ActorManagement.Repositories.Contracts.AllContracts;
using ActorManagement.Services.Contracts.AllContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Services.AllServices
{
    public class BiographyService : IBiographyService
    {
        private readonly IUnitRepository _unitRepository;
        public BiographyService(IUnitRepository unitRepository)
        {
            this._unitRepository = unitRepository;
        }

        public async Task AddBiographyAsync(Biography biography)
        {
            await _unitRepository.BiographyRepository.AddAsync(biography);
        }

        public async Task<Biography?> GetBiographyByActorIdAsync(Guid? actorId)
        {
            return await _unitRepository.BiographyRepository.GetBiographyByActorIdAsync(actorId);
        }

        public async Task<Biography?> GetBiographyByIdAsync(Guid? biographyId)
        {
            return await _unitRepository.BiographyRepository.GetByIdAsync(biographyId);
        }

        public void RemoveImageFromBiography(BiographyImage existingImage)
        {
            _unitRepository.BiographyRepository.RemoveImageFromBiography(existingImage);
        }

        public void UpdateBiography(Biography existBiography)
        {
            _unitRepository.BiographyRepository.Update(existBiography);
        }
    }
}
