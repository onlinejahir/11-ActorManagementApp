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
    public class BiographyImageRepository : GenericRepository<BiographyImage>, IBiographyImageRepository
    {
        public readonly AppDbContext _dbContext;
        public BiographyImageRepository(AppDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public override void Remove(BiographyImage entity)
        {
            _dbContext.BiographyImages.Remove(entity);
        }
    }
}
