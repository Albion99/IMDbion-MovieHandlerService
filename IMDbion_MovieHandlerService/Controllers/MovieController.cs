using IMDbion_MovieHandlerService.DataContext;
using Microsoft.AspNetCore.Mvc;
using IMDbion_MovieHandlerService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using IMDbion_MovieHandlerService.Services;
using IMDbion_MovieHandlerService.Mappers;
using AutoMapper;
using IMDbion_MovieHandlerService.DTO;

namespace IMDbion_MovieHandlerService.Controllers
{
    [ApiController]
    [Route("movies")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;

        public MovieController(IMovieService movieService, IMapper mapper)
        {
            _movieService = movieService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<MovieDTO>> GetMovies()
        {
            var movies = await _movieService.GetAllMovies();
            return _mapper.Map<List<MovieDTO>>(movies);
        }

        [HttpGet("{movieId}")]
        public async Task<MovieDTO> GetSelectedMovie(Guid movieId)
        {
            Movie movie = await _movieService.GetMovie(movieId);
            return _mapper.Map<MovieDTO>(movie);
        }

        [HttpPost]
        public async Task<Movie> AddMovie([FromBody] MovieDTO movieDTO)
        {
            Movie movie = _mapper.Map<Movie>(movieDTO);
            return await _movieService.Create(movie);
        }

        [HttpPut("{movieId}")]
        public async Task<Movie> UpdateMovie(Guid movieId, [FromBody] MovieDTO movieDTO)
        {
            Movie movie = _mapper.Map<Movie>(movieDTO);
            return await _movieService.Update(movieId, movie);
        }

        [HttpDelete("{movieId}")]
        public async Task DeleteMovie(Guid movieId)
        {
            await _movieService.Delete(movieId);
        }
    }
}