using IMDbion_MovieHandlerService.DataContext;
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

        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            return await _movieContext.Movies.ToListAsync();
        }

        public async Task<ActionResult<Movie>> GetMovie(int movieId)
        {
            var foundMovie = await _movieContext.Movies.FindAsync(movieId);

            if (foundMovie == null)
            {
                throw new Exception("Movie with id:" + movieId + " was not found");
            }

            return foundMovie;
        }

        public async Task<ActionResult<Movie>> Create(Movie movie)
        {
            _movieContext.Movies.Add(movie);
            await _movieContext.SaveChangesAsync();

            return await GetMovie(movie.Id);
        }

        public async Task<ActionResult<Movie>> Update(int movieId, Movie movie)
        {
            if (movieId != movie.Id)
            {
                throw new Exception("Movie with id:" + movieId + " was not found");
            }

            _movieContext.Entry(movie).State = EntityState.Modified;
            await _movieContext.SaveChangesAsync();

            return await GetMovie(movieId);
        }

        public async void Delete(int movieId)
        {
            var foundMovie = await _movieContext.Movies.FindAsync(movieId);

            if (foundMovie == null)
            {
                throw new Exception("Movie with id:" + movieId + " was not found");
            }

            _movieContext.Movies.Remove(foundMovie);
            await _movieContext.SaveChangesAsync();
        }
    }
}
