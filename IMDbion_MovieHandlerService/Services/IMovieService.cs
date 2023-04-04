using IMDbion_MovieHandlerService.Models;
using Microsoft.AspNetCore.Mvc;

namespace IMDbion_MovieHandlerService.Services
{
    public interface IMovieService
    {
        public Task<IEnumerable<Movie>> GetAllMovies();
        public Task<Movie> GetMovie(Guid movieId);
        public Task<Movie> Create(Movie movie);
        public Task<Movie> Update(Guid movieId, Movie movie);
        public Task Delete(Guid movieId);
    }
}
