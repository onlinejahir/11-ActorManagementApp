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
    public class BiographyImageService : IBiographyImageService
    {
        private readonly IUnitRepository _unitRepository;
        public BiographyImageService(IUnitRepository unitRepository)
        {
            this._unitRepository = unitRepository;
        }

        public void RemoveBiographyImage(BiographyImage existingImage)
        {
            _unitRepository.BiographyImageRepository.Remove(existingImage);
        }
    }
}
