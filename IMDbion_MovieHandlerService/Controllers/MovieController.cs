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
        private readonly MovieService _movieService;

        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _movieService.GetAllMovies();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetSelectedMovie(int movieId)
        {
            return await _movieService.GetMovie(movieId);
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> AddMovie(Movie movie)
        {
            return await _movieService.Create(movie);  
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Movie>> UpdateMovie(int id, Movie movie)
        {
            return await _movieService.Update(id, movie);
        }

        [HttpDelete("{id}")]
        public void DeleteMovie(int id)
        {
            _movieService.Delete(id);
        }
    }
}