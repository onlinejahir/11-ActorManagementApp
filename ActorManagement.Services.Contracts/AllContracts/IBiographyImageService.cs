using ActorManagement.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Services.Contracts.AllContracts
{
    public interface IBiographyImageService
    {
        void RemoveBiographyImage(BiographyImage existingImage);
    }
}
