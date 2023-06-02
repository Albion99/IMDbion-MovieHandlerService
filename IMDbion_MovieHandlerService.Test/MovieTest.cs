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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using IMDbion_MovieHandlerService.RabbitMQ;

namespace IMDbion_MovieHandlerService.Test
{
    public class MovieTest
    {
        private Mock<MovieContext> _mockMovieContext;
        private MovieService _movieService;
        private Mock<IRabbitMQRetriever<List<Actor>>> _mockRabbitMQRetriever;

        [SetUp]
        public void Setup()
        {
            _mockMovieContext = new Mock<MovieContext>();
            _mockRabbitMQRetriever = new Mock<IRabbitMQRetriever<List<Actor>>>();
            _movieService = new MovieService(_mockMovieContext.Object, _mockRabbitMQRetriever.Object);
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
                    VideoPath = "path",
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
                    VideoPath = "path",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            var mock = _movieList.BuildMock().BuildMockDbSet();
            _mockMovieContext.Setup(x => x.Movies).Returns(mock.Object);

            // Act
            var result = await _movieService.GetMovies(1, 1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Should_Get_Selected_Movie()
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
                VideoPath = "path",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            List<Guid> actorIds = new()
            {
                Guid.NewGuid()
            };

            List<MovieActor> movieActors = new()
            {
                new MovieActor
                {
                    MovieId = movie.Id,
                    ActorId = actorIds.First(),
                }
            };

            var mock = movieActors.BuildMock().BuildMockDbSet();
            _mockMovieContext.Setup(x => x.MovieActors).Returns(mock.Object);

            _mockMovieContext.Setup(x => x.Movies.FindAsync(movie.Id).Result).Returns(movie);

            // Act
            var result = await _movieService.GetMovie(movie.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(movie.Id));

            _mockMovieContext.Verify(x => x.Movies.FindAsync(movie.Id), Times.Once());
        }

        [Test]
        public async Task Should_Throw_Not_Found_Exception_If_Movie_Doesnt_Exist()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            _mockMovieContext.Setup(x => x.Movies.FindAsync(guid)).ReturnsAsync((Movie)null);

            // Act
            Assert.ThrowsAsync<NotFoundException>(async () => await _movieService.GetMovie(guid));
        }

        [Test]
        public async Task Should_Create_Movie_And_Return_Movie()
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
                VideoPath = "path",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            List<Guid> actorIds = new()
            {
                Guid.NewGuid()
            };

            List<MovieActor> movieActors = new()
            {
                new MovieActor
                {
                    MovieId = movie.Id,
                    ActorId = actorIds.First(),
                }
            };

            var mock = movieActors.BuildMock().BuildMockDbSet();
            _mockMovieContext.Setup(x => x.MovieActors).Returns(mock.Object);

            _mockMovieContext.Setup(x => x.Movies.Add(movie));
            _mockMovieContext.Setup(x => x.AddRange(movieActors));
            _mockMovieContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);
            _mockMovieContext.Setup(x => x.Movies.FindAsync(movie.Id).Result).Returns(movie);

            // Act
            var result = await _movieService.Create(movie, actorIds);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(movie));

            _mockMovieContext.Verify(x => x.Movies.Add(movie), Times.Once());
            _mockMovieContext.Verify(x => x.SaveChangesAsync(default), Times.Once());
            _mockMovieContext.Verify(x => x.Movies.FindAsync(movie.Id), Times.Once());
            _mockMovieContext.Verify(x => x.AddRange(It.IsAny<IEnumerable<MovieActor>>()), Times.Once());
        }

        [Test]
        public void Should_Throw_Cant_Be_Null_Exception_If_Creating_Movie_Is_Null()
        {
            // Arrange
            Movie movie = null;
            List<Guid> actorIds = new();

            // Act
            Assert.ThrowsAsync<CantBeNullException>(() => _movieService.Create(movie, actorIds));
        }

        [Test]
        public async Task Should_Update_Movie_And_Return_Movie()
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
                VideoPath = "path",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            List<Guid> actorIds = new()
            {
                Guid.NewGuid()
            };

            List<MovieActor> movieActors = new()
            {
                new MovieActor
                {
                    MovieId = movie.Id,
                    ActorId = actorIds.First(),
                }
            };

            _mockMovieContext.Setup(x => x.Movies.FindAsync(movie.Id)).ReturnsAsync(movie);
            _mockMovieContext.Setup(x => x.Movies.Update(movie)).Returns(Mock.Of<EntityEntry<Movie>>);
            _mockMovieContext.Setup(x => x.AddRange(movieActors));

            var mock = movieActors.BuildMock().BuildMockDbSet();
            _mockMovieContext.Setup(x => x.MovieActors).Returns(mock.Object);

            _mockMovieContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            movie.Title = "Test";

            // Act
            var result = await _movieService.Update(movie.Id, movie, actorIds);

            // Assert
            Assert.That(result.Id, Is.EqualTo(movie.Id));
            Assert.That(result.Title, Is.EqualTo(movie.Title));

            _mockMovieContext.Verify(x => x.Movies.FindAsync(movie.Id), Times.Once());
            _mockMovieContext.Verify(x => x.Update(movie), Times.Once());
            _mockMovieContext.Verify(x => x.SaveChangesAsync(default), Times.Once());
            _mockMovieContext.Verify(x => x.AddRange(It.IsAny<IEnumerable<MovieActor>>()), Times.Once());
        }

        [Test]
        public void Should_Throw_Cant_Be_Null_Exception_If_Updating_Movie_Is_Null()
        {
            // Arrange
            Movie movie = null;
            Guid guid = Guid.NewGuid();
            List<Guid> actorIds = new();
            // Act
            Assert.ThrowsAsync<CantBeNullException>(() => _movieService.Update(guid, movie, actorIds));
        }

        [Test]
        public async Task Should_Delete_Selected_Movie()
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
                VideoPath = "path",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            List<Guid> actorIds = new()
            {
                Guid.NewGuid()
            };

            List<MovieActor> movieActors = new()
            {
                new MovieActor
                {
                    MovieId = movie.Id,
                    ActorId = actorIds.First(),
                }
            };

            var mock = movieActors.BuildMock().BuildMockDbSet();
            _mockMovieContext.Setup(x => x.MovieActors).Returns(mock.Object);

            _mockMovieContext.Setup(x => x.Movies.FindAsync(movie.Id)).ReturnsAsync(movie);
            _mockMovieContext.Setup(x => x.Movies.Remove(movie));
            _mockMovieContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            await _movieService.Delete(movie.Id);

            // Arrange
            _mockMovieContext.Verify(x => x.Movies.FindAsync(movie.Id), Times.Exactly(2));
            _mockMovieContext.Verify(x => x.Movies.Remove(movie), Times.Once);
            _mockMovieContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Test]
        public void Should_Throw_Not_Found_Exception_If_Deleting_Non_Existant_Movie()
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
                VideoPath = "path",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _mockMovieContext.Setup(x => x.Movies.Add(movie));

            Guid guid = Guid.NewGuid();

            // Act
            Assert.ThrowsAsync<NotFoundException>(() => _movieService.Delete(guid));
        }
    }
}