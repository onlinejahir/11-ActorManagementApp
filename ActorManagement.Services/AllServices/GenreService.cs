using ActorManagement.Repositories.Contracts.AllContracts;
using ActorManagement.Services.Contracts.AllContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Services.AllServices
{
    public class GenreService : IGenreService
    {
        private readonly IUnitRepository _unitRepository;
        public GenreService(IUnitRepository unitRepository)
        {
            this._unitRepository = unitRepository;
        }

    }
}
