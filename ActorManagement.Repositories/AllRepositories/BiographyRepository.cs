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
    public class BiographyRepository : GenericRepository<Biography>, IBiographyRepository
    {
        private readonly AppDbContext _dbContext;
        public BiographyRepository(AppDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Biography?> GetBiographyByActorIdAsync(Guid? actorId)
        {
            return await _dbContext.Biographies
                .Include(b => b.Actor)
                .Include(b => b.BiographyImages)
                .FirstOrDefaultAsync(b => b.ActorId == actorId);
        }

        public override async Task<Biography?> GetByIdAsync(Guid? id)
        {
            return await _dbContext.Biographies
                .Include(b => b.Actor)
                .Include(b => b.BiographyImages)
                .FirstOrDefaultAsync(b => b.BiographyId == id);
        }

        public void RemoveImageFromBiography(BiographyImage existingImage)
        {
            var biography = _dbContext.Biographies
                                       .Include(b => b.BiographyImages)
                                       .FirstOrDefault(b => b.BiographyId == existingImage.BiographyId);

            if (biography != null)
            {
                // Remove the image from the Biography's BiographyImages collection
                var imageToRemove = biography.BiographyImages.FirstOrDefault(img => img.BiographyImageId == existingImage.BiographyImageId);
                if (imageToRemove != null)
                {
                    //biography.BiographyImages.Remove(imageToRemove);
                    _dbContext.BiographyImages.Remove(imageToRemove);  // Also remove it from the DbSet to delete the record
                }
            }
        }
    }
}
