using IMDbion_MovieHandlerService.Models;
using Microsoft.AspNetCore.Mvc;

namespace IMDbion_MovieHandlerService.Services
{
    public interface IMovieService
    {
        public Task<IEnumerable<Movie>> GetMovies(int pageSize, int pageNumber);
        public Task<int> GetTotalMoviesCount();
        public Task<Movie> GetMovie(Guid movieId);
        public Task<Movie> Create(Movie movie, List<Guid> actorIds);
        public Task<Movie> Update(Guid movieId, Movie movie, List<Guid> actorIds);
        public Task Delete(Guid movieId);
    }
}
