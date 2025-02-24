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
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        private readonly AppDbContext _dbContext;
        public GenreRepository(AppDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

    }
}
