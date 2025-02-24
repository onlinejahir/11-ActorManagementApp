using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Services.Contracts.AllContracts
{
    public interface IUnitService : IDisposable, IAsyncDisposable
    {
        IActorService ActorService { get; }
        IBiographyService BiographyService { get; }
        IGenreService GenreService { get; }
        IMovieService MovieService { get; }
        IBiographyImageService BiographyImageService { get; }
        Task<bool> SaveChangesAsync();
    }
}
