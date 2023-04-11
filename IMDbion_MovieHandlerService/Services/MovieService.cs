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
             return await _movieContext.Movies.FindAsync(movieId) ?? throw new NotFoundException("Movie with id: " + movieId + " does not exist");
        }

        public async Task<Movie> Create(Movie movie)
        {
            if (movie == null)
            {
                throw new FieldNullException("Movie can't be empty!");
            }

            _movieContext.Movies.Add(movie);
            await _movieContext.SaveChangesAsync();

            return await GetMovie(movie.Id);
        }

        public async Task<Movie> Update(Guid movieId, Movie movie)
        {
            if (movie == null)
            {
                throw new FieldNullException("Movie can't be empty!");
            }

            movie.Id = movieId;

            _movieContext.Entry(movie).State = EntityState.Modified;
            await _movieContext.SaveChangesAsync();

            return await GetMovie(movie.Id);
        }

        public async Task Delete(Guid movieId)
        {
            var foundMovie = await _movieContext.Movies.FindAsync(movieId);

            if (foundMovie == null)
            {
                throw new NotFoundException("Movie with id: " + movieId + " does not exist");
            }

            _movieContext.Movies.Remove(foundMovie);
            await _movieContext.SaveChangesAsync();
        }
    }
}
