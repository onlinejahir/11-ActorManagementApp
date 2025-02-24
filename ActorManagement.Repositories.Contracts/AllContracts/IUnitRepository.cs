using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Repositories.Contracts.AllContracts
{
    public interface IUnitRepository : IDisposable, IAsyncDisposable
    {
        IActorRepository ActorRepository { get; }
        IBiographyRepository BiographyRepository { get; }
        IGenreRepository GenreRepository { get; }
        IMovieRepository MovieRepository { get; }
        IBiographyImageRepository BiographyImageRepository { get; }
        Task<bool> SaveChangesAsync();
    }
}
