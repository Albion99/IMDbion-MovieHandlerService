using IMDbion_MovieHandlerService.DataContext;
using Microsoft.AspNetCore.Mvc;
using IMDbion_MovieHandlerService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using IMDbion_MovieHandlerService.Services;

namespace IMDbion_MovieHandlerService.Controllers
{
    [ApiController]
    [Route("movies")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IEnumerable<Movie>> GetMovies()
        {
            return await _movieService.GetAllMovies();
        }

        [HttpGet("{id}")]
        public async Task<Movie> GetSelectedMovie(Guid movieId)
        {
            return await _movieService.GetMovie(movieId);
        }

        [HttpPost]
        public async Task<Movie> AddMovie([FromBody] Movie movie)
        {
            return await _movieService.Create(movie);  
        }

        [HttpPut("{id}")]
        public async Task<Movie> UpdateMovie(Guid movieId, [FromBody] Movie movie)
        {
            return await _movieService.Update(movieId, movie);
        }

        [HttpDelete("{id}")]
        public async Task DeleteMovie(Guid id)
        {
            await _movieService.Delete(id);
        }
    }
}