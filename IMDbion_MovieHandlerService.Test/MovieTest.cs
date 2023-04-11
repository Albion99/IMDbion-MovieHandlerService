using IMDbion_MovieHandlerService.Services;
using IMDbion_MovieHandlerService.Models;
using IMDbion_MovieHandlerService.Mappers;
using IMDbion_MovieHandlerService.DTOs;
using IMDbion_MovieHandlerService.DataContext;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace IMDbion_MovieHandlerService.Test
{
    public class MovieTest
    {
        private Mock<MovieContext> _mockMovieContext;
        private MovieService _movieService;

        [SetUp]
        public void Setup()
        {
            _mockMovieContext = new Mock<MovieContext>();
            _movieService = new MovieService(_mockMovieContext.Object);
        }

        [Test]
        public async Task Should_Get_All_Movies()
        {
            // Arrange
            IEnumerable<Movie> movies = new List<Movie>() {
                new Movie {
                    Id = Guid.NewGuid(),
                    Title = "The Shawshank Redemption",
                    Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                    Genre = "Drama",
                    Length = 142,
                    PublicationDate = new DateTime(1994, 9, 23),
                    CountryOfOrigin = "USA",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Movie {
                    Id = Guid.NewGuid(),
                    Title = "The Godfather",
                    Description = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
                    Genre = "Crime",
                    Length = 175,
                    PublicationDate = new DateTime(1972, 3, 24),
                    CountryOfOrigin = "USA",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            // Act
            var result = await _movieService.GetAllMovies();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(movies.Count()));
        }
    }
}