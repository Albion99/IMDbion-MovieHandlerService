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
    }
}
