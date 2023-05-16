using AutoMapper;
using IMDbion_MovieHandlerService.DataContext;
using IMDbion_MovieHandlerService.DTOs;
using IMDbion_MovieHandlerService.Exceptions;
using IMDbion_MovieHandlerService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace IMDbion_MovieHandlerService.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieContext _movieContext;
        private readonly IMapper _mapper;

        public MovieService(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            return await _movieContext.Movies.ToListAsync();
        }

        public async Task<Movie> GetMovie(Guid movieId)
        {
            Movie movie = await _movieContext.Movies.FindAsync(movieId);

            if (movie == null)
            {
                throw new NotFoundException("Movie with id: " + movieId + " does not exist");
            }

            movie.Actors = GetMovieActors(movieId);

            return movie;
        }


        public async Task<Movie> Create(Movie movie, List<Guid> actorIds)
        {
            if (movie == null)
            {
                throw new CantBeNullException("Movie can't be empty!");
            }

            _movieContext.Movies.Add(movie);
            InsertMovieActors(movie, actorIds);
            await _movieContext.SaveChangesAsync();

            return await GetMovie(movie.Id);
        }

        public async Task<Movie> Update(Guid movieId, Movie movie, List<Guid> actorIds)
        {
            if (movie == null)
            {
                throw new CantBeNullException("Movie can't be empty!");
            }

            movie.Id = movieId;

            _movieContext.Update(movie);
            DeleteMovieActors(movie);
            InsertMovieActors(movie, actorIds);
            await _movieContext.SaveChangesAsync();

            return await GetMovie(movie.Id);
        }

        public async Task Delete(Guid movieId)
        {
            DeleteMovieActors(await GetMovie(movieId));
            _movieContext.Movies.Remove(await GetMovie(movieId));
            await _movieContext.SaveChangesAsync();
        }

        private void InsertMovieActors(Movie movie, List<Guid> actorIds)
        {
            List<MovieActor> movieActors = actorIds.Select(actorId => new MovieActor
            {
                MovieId = movie.Id,
                ActorId = actorId
            }).ToList();

            _movieContext.AddRange(movieActors);
        }

        private void DeleteMovieActors(Movie movie)
        {
            List<MovieActor> movieActorsToRemove = new();

            foreach (var movieActor in _movieContext.MovieActors)
            {
                if (movieActor.MovieId == movie.Id)
                {
                    movieActorsToRemove.Add(movieActor);
                }
            }

            foreach (var movieActorToRemove in movieActorsToRemove)
            {
                _movieContext.MovieActors.Remove(movieActorToRemove);
            }
        }

        private List<MovieActor> GetMovieActors(Guid movieId)
        {
            List<MovieActor> movieWithActors = new();

            foreach (var movieActor in _movieContext.MovieActors)
            {
                if (movieActor.MovieId == movieId)
                {
                    movieWithActors.Add(movieActor);
                }
            }

            return movieWithActors;
        }
    }
}
