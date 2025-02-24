using ActorManagement.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Database.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Biography> Biographies { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BiographyImage> BiographyImages { get; set; }
        public DbSet<ActorMovie> ActorMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActorMovie>()
                .ToTable("ActorMovies")
                .HasKey(am => new { am.ActorId, am.MovieId });
            modelBuilder.Entity<ActorMovie>()
                .HasOne(am => am.Actor)
                .WithMany(am => am.ActorMovies)
                .HasForeignKey(am => am.ActorId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ActorMovie>()
                .HasOne(am => am.Movie)
                .WithMany(am => am.ActorMovies)
                .HasForeignKey(am => am.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Biography>()
                .HasMany(b => b.BiographyImages)
                .WithOne(bi => bi.Biography)
                .HasForeignKey(bi => bi.BiographyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
