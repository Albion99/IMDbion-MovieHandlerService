using IMDbion_MovieHandlerService.DataContext;
using Microsoft.AspNetCore.Mvc;
using IMDbion_MovieHandlerService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using IMDbion_MovieHandlerService.Services;
using IMDbion_MovieHandlerService.Mappers;
using AutoMapper;
using IMDbion_MovieHandlerService.DTOs;
using System.IdentityModel.Tokens.Jwt;
using IMDbion_MovieHandlerService.Exceptions;

namespace IMDbion_MovieHandlerService.Controllers
{
    [ApiController]
    [Route("")]
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
        public async Task<ActionResult<MovieListDTO>> GetMovies(int pageSize = 10, int pageNumber = 1)
        {
            IEnumerable<Movie> movies = await _movieService.GetMovies(pageSize, pageNumber);
            List<MovieDTO> movieDTOs = _mapper.Map<List<MovieDTO>>(movies);

            int totalMoviesCount = await _movieService.GetTotalMoviesCount();
            int totalPages = (int)Math.Ceiling((double)totalMoviesCount / pageSize);

            MovieListDTO movieListDTO = new()
            {
                Movies = movieDTOs,
                TotalPages = totalPages
            };

            return movieListDTO;
        }

        [HttpGet("{movieId}")]
        public async Task<MovieDTO> GetSelectedMovie(Guid movieId)
        {
            Movie movie = await _movieService.GetMovie(movieId);
            return _mapper.Map<MovieDTO>(movie);
        }

        [HttpPost]
        public async Task<MovieDTO> AddMovie([FromBody] MovieCreateDTO movieCreateDTO)
        {
            IsAdmin();

            Movie movie = _mapper.Map<Movie>(movieCreateDTO);

            await _movieService.Create(movie, movieCreateDTO.ActorIds);

            MovieDTO movieDTO = _mapper.Map<MovieDTO>(movie);
            return movieDTO;
        }

        [HttpPut("{movieId}")]
        public async Task<MovieDTO> UpdateMovie(Guid movieId, [FromBody] MovieUpdateDTO movieUpdateDTO)
        {
            IsAdmin();

            Movie movie = _mapper.Map<Movie>(movieUpdateDTO);

            Movie updatedMovie = await _movieService.Update(movieId, movie, movieUpdateDTO.ActorIds);

            MovieDTO movieDTO = _mapper.Map<MovieDTO>(updatedMovie);
            return movieDTO;
        }

        [HttpDelete("{movieId}")]
        public async Task DeleteMovie(Guid movieId)
        {
            IsAdmin();

            await _movieService.Delete(movieId);
        }

        private void IsAdmin()
        {
            string tokenString = Request.Headers.TryGetValue("Authorization", out var headerValue)
                ? headerValue.ToString().Replace("Bearer ", "")
                : throw new InvalidJWTTokenException("Missing or invalid Authorization header!");

            JwtSecurityTokenHandler tokenHandler = new();

            JwtSecurityToken token = tokenHandler.ReadJwtToken(tokenString) ?? throw new InvalidJWTTokenException("Invalid JWT token.");
            bool isAdmin = token.Claims.Any(claim => claim.Type == "https://s6albion.albionz.nl/roles" && claim.Value == "Admin");

            if (isAdmin)
            {
                throw new NotAuthorizedException("Logged in user is not an Admin!");
            }
        }
    }
}