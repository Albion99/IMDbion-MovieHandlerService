﻿using IMDbion_MovieHandlerService.DataContext;
using Microsoft.AspNetCore.Mvc;
using IMDbion_MovieHandlerService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using IMDbion_MovieHandlerService.Services;
using IMDbion_MovieHandlerService.Mappers;
using AutoMapper;
using IMDbion_MovieHandlerService.DTOs;

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
        public async Task<List<MovieDTO>> GetMovies()
        {
            IEnumerable<Movie> movies = await _movieService.GetAllMovies();
            return _mapper.Map<List<MovieDTO>>(movies);
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
            Movie movie = _mapper.Map<Movie>(movieCreateDTO);

            await _movieService.Create(movie, movieCreateDTO.ActorIds);

            MovieDTO movieDTO = _mapper.Map<MovieDTO>(movie);
            return movieDTO;
        }

        [HttpPut("{movieId}")]
        public async Task<MovieDTO> UpdateMovie(Guid movieId, [FromBody] MovieCreateDTO movieCreateDTO)
        {
            Movie movie = _mapper.Map<Movie>(movieCreateDTO);

            List<Guid> actorIds = movieCreateDTO.ActorIds;

            await _movieService.Update(movieId, movie, actorIds);

            MovieDTO movieDTO = _mapper.Map<MovieDTO>(movie);
            return movieDTO;
        }

        [HttpDelete("{movieId}")]
        public async Task DeleteMovie(Guid movieId)
        {
            await _movieService.Delete(movieId);
        }
    }
}