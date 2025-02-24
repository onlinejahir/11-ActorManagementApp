using ActorManagement.Database.Data;
using ActorManagement.Models.EntityModels;
using ActorManagement.Repositories.Contracts.AllContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Repositories.AllRepositories
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        private readonly AppDbContext _dbContext;
        public MovieRepository(AppDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

    }
}
