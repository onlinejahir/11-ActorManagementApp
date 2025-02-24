using ActorManagement.Repositories.Contracts.AllContracts;
using ActorManagement.Services.Contracts.AllContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Services.AllServices
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _unitRepository;
        public UnitService(IUnitRepository unitRepository)
        {
            this._unitRepository = unitRepository;
        }
        public IActorService ActorService => new ActorService(this._unitRepository);

        public IBiographyService BiographyService => new BiographyService(this._unitRepository);

        public IGenreService GenreService => new GenreService(this._unitRepository);

        public IMovieService MovieService => new MovieService(this._unitRepository);

        public IBiographyImageService BiographyImageService => new BiographyImageService(this._unitRepository);

        public async Task<bool> SaveChangesAsync()
        {
            return await _unitRepository.SaveChangesAsync();
        }

        public void Dispose()
        {
            this._unitRepository.Dispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (_unitRepository is IAsyncDisposable asyncDisposableRepo)
            {
                await asyncDisposableRepo.DisposeAsync(); //Dispose asynchronously
            }
            else
            {
                _unitRepository.Dispose(); //Fallback to synchronous disposal
            }
            GC.SuppressFinalize(this);
        }
    }
}
