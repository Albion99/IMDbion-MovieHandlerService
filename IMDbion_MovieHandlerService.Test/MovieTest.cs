using IMDbion_MovieHandlerService.Services;
using IMDbion_MovieHandlerService.Models;
using IMDbion_MovieHandlerService.Mappers;
using IMDbion_MovieHandlerService.DTOs;
using IMDbion_MovieHandlerService.DataContext;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using MockQueryable.Moq;
using IMDbion_MovieHandlerService.Exceptions;

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
            IEnumerable<Movie> _movieList = new List<Movie>() {
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
            var mock = _movieList.BuildMock().BuildMockDbSet();
            _mockMovieContext.Setup(x => x.Movies).Returns(mock.Object);

            // Act
            var result = await _movieService.GetAllMovies();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task Should_Get_One_Movie()
        {
            // Arrange
            Movie movie = new() {
                Id = Guid.NewGuid(),
                Title = "The Shawshank Redemption",
                Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                Genre = "Drama",
                Length = 142,
                PublicationDate = new DateTime(1994, 9, 23),
                CountryOfOrigin = "USA",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _mockMovieContext.Setup(x => x.Movies.FindAsync(movie.Id).Result).Returns(movie);

            // Act
            var result = await _movieService.GetMovie(movie.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(movie.Id));
        }

        [Test]
        public async Task Should_Throw_Not_Found_Exception()
        {
            // Arrange
            Movie movie = new()
            {
                Id = Guid.NewGuid(),
                Title = "The Shawshank Redemption",
                Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                Genre = "Drama",
                Length = 142,
                PublicationDate = new DateTime(1994, 9, 23),
                CountryOfOrigin = "USA",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            Guid guid = Guid.NewGuid();
            _mockMovieContext.Setup(x => x.Movies.FindAsync(movie.Id).Result).Throws<NotFoundException>();

            // Act
            var result = Assert.ThrowsAsync<NotFoundException>(() => _movieService.GetMovie(guid));
        }

        [Test]
        public async Task Should_Add_Movie()
        {
            // Arrange
            Movie movie = new()
            {
                Id = Guid.NewGuid(),
                Title = "The Shawshank Redemption",
                Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                Genre = "Drama",
                Length = 142,
                PublicationDate = new DateTime(1994, 9, 23),
                CountryOfOrigin = "USA",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _mockMovieContext.Setup(x => x.Movies.Add(movie));
            _mockMovieContext.Setup(x => x.Movies.FindAsync(movie.Id).Result).Returns(movie);

            // Act
            var result = _movieService.Create(movie);

            // Arrange
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result.Title, Is.EqualTo(movie.Title));
            Assert.That(result.Result.Description, Is.EqualTo(movie.Description));
            Assert.That(result.Result.Genre, Is.EqualTo(movie.Genre));
            Assert.That(result.Result.Length, Is.EqualTo(movie.Length));
            Assert.That(result.Result.PublicationDate, Is.EqualTo(movie.PublicationDate));
            Assert.That(result.Result.CountryOfOrigin, Is.EqualTo(movie.CountryOfOrigin));
            Assert.That(result.Result.CreatedAt, Is.EqualTo(movie.CreatedAt));
            Assert.That(result.Result.UpdatedAt, Is.EqualTo(movie.UpdatedAt));

        }
    }
}