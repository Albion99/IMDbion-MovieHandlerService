using IMDbion_MovieHandlerService.DataContext;
using IMDbion_MovieHandlerService.Exceptions;
using IMDbion_MovieHandlerService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace IMDbion_MovieHandlerService.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieContext _movieContext;

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

            return movie;
        }

        public async Task<Movie> Create(Movie movie, List<Guid> actorIds)
        {
            if (movie == null)
            {
                throw new CantBeNullException("Movie can't be empty!");
            }

            _movieContext.Movies.Add(movie);
            InsertMovieActor(movie, actorIds);
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
            InsertMovieActor(movie, actorIds);
            await _movieContext.SaveChangesAsync();

            return await GetMovie(movie.Id);
        }

        public async Task Delete(Guid movieId)
        {
            _movieContext.Movies.Remove(await GetMovie(movieId));
            await _movieContext.SaveChangesAsync();
        }

        private void InsertMovieActor(Movie movie, List<Guid> actorIds)
        {
            List<MovieActor> movieActors = actorIds.Select(actorId => new MovieActor
            {
                MovieId = movie.Id,
                ActorId = actorId
            }).ToList();

            _movieContext.AddRange(movieActors);
        }
    }
}
