using ActorManagement.Database.Data;
using ActorManagement.Repositories.Contracts.AllContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Repositories.AllRepositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly AppDbContext _dbContext;
        public UnitRepository(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public IActorRepository ActorRepository => new ActorRepository(this._dbContext);

        public IBiographyRepository BiographyRepository => new BiographyRepository(this._dbContext);

        public IGenreRepository GenreRepository => new GenreRepository(this._dbContext);

        public IMovieRepository MovieRepository => new MovieRepository(this._dbContext);

        public IBiographyImageRepository BiographyImageRepository => new BiographyImageRepository(this._dbContext);

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            this._dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (_dbContext != null)
            {
                await this._dbContext.DisposeAsync();
                GC.SuppressFinalize(this);
            }
        }
    }
}
