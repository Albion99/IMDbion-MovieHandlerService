using IMDbion_MovieHandlerService.Models;
using Microsoft.EntityFrameworkCore;

namespace IMDbion_MovieHandlerService.DataContext
{
    public class MovieContext: DbContext
    {
        public MovieContext()
        {
        }

        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MovieActor> MovieActors { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieActor>()
                .Property(m => m.MovieId)
                .HasColumnType("char(36)");

            modelBuilder.Entity<MovieActor>()
                .Property(m => m.ActorId)
                .HasColumnType("char(36)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
