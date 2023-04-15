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
using System.Formats.Asn1;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        public async Task Should_Get_Selected_Movie()
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

            _mockMovieContext.Verify(x => x.Movies.FindAsync(movie.Id), Times.Once());
        }

        [Test]
        public async Task Should_Throw_Not_Found_Exception()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            _mockMovieContext.Setup(x => x.Movies.FindAsync(guid)).ReturnsAsync((Movie)null);

            // Act
            Assert.ThrowsAsync<NotFoundException>(async () => await _movieService.GetMovie(guid));
        }

        [Test]
        public async Task Should_Add_Movie_And_Return_Movie()
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

            _mockMovieContext.Setup(x => x.Movies.Add(movie));
            _mockMovieContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);
            _mockMovieContext.Setup(x => x.Movies.FindAsync(movie.Id).Result).Returns(movie);

            // Act
            var result = await _movieService.Create(movie);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(movie));

            _mockMovieContext.Verify(x => x.Movies.Add(movie), Times.Once());
            _mockMovieContext.Verify(x => x.SaveChangesAsync(default), Times.Once());
            _mockMovieContext.Verify(x => x.Movies.FindAsync(movie.Id), Times.Once());
        }

        [Test]
        public async Task Should_Throw_Cant_Be_Null_Exception()
        {
            // Arrange
            Movie movie = null;

            // Act
            Assert.ThrowsAsync<CantBeNullException>(() => _movieService.Create(movie));
        }

        [Test]
        public async Task Should_Update_Movie_And_Return_Movie()
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

            _mockMovieContext.Setup(x => x.Movies.FindAsync(movie.Id)).ReturnsAsync(movie);
            _mockMovieContext.Setup(x => x.Movies.Update(movie)).Returns(Mock.Of<EntityEntry<Movie>>);
            _mockMovieContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            movie.Title = "Test";

            // Act
            var result = await _movieService.Update(movie.Id, movie);

            // Assert
            Assert.That(result.Id, Is.EqualTo(movie.Id));
            Assert.That(result.Title, Is.EqualTo(movie.Title));

            _mockMovieContext.Verify(x => x.Movies.FindAsync(movie.Id), Times.Once());
            _mockMovieContext.Verify(x => x.Update(movie), Times.Once());
            _mockMovieContext.Verify(x => x.SaveChangesAsync(default), Times.Once());
        }

        [Test]
        public async Task Should_Delete_Selected_Movie()
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

            _mockMovieContext.Setup(x => x.Movies.FindAsync(movie.Id)).ReturnsAsync(movie);
            _mockMovieContext.Setup(x => x.Movies.Remove(movie));
            _mockMovieContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            await _movieService.Delete(movie.Id);

            // Arrange
            _mockMovieContext.Verify(x => x.Movies.FindAsync(movie.Id), Times.Once());
            _mockMovieContext.Verify(x => x.Movies.Remove(movie), Times.Once);
            _mockMovieContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}