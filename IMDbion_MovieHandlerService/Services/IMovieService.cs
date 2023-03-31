using IMDbion_MovieHandlerService.Models;
using Microsoft.AspNetCore.Mvc;

namespace IMDbion_MovieHandlerService.Services
{
    public interface IMovieService
    {
        public Task<ActionResult<IEnumerable<Movie>>> GetAllMovies();
        public Task<ActionResult<Movie>> GetMovie(int movieId);
        public Task<ActionResult<Movie>> Create(Movie movie);
        public Task<ActionResult<Movie>> Update(int movieId, Movie movie);
        public void Delete(int movieId);
    }
}
