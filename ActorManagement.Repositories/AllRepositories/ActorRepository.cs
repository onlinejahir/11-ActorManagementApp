using ActorManagement.Database.Data;
using ActorManagement.Models.EntityModels;
using ActorManagement.Repositories.Contracts.AllContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Repositories.AllRepositories
{
    public class ActorRepository : GenericRepository<Actor>, IActorRepository
    {
        private readonly AppDbContext _dbContext;
        public ActorRepository(AppDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Actor?> GetActorByEmailAsync(string email)
        {
            return await _dbContext.Actors.FirstOrDefaultAsync(a => a.Email == email);
        }
    }
}
