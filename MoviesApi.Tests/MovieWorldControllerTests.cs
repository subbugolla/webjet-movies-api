using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using MoviesApi.Controllers;
using MoviesApi.Interfaces;
using MoviesApi.Models;

namespace MoviesApi.Tests
{
    public class MovieworldControllerTests
    {
        private readonly Mock<ICinemaworldService> _mockCinemaworldService;
        private readonly Mock<IFilmworldService> _mockFilmworldService;
        private readonly Mock<ILogger<MovieWorldController>> _mockLogger;
        private readonly IMovieWorldController _controller;
        private readonly Mock<IDistributedCache> _mockDistributedCache;

        public MovieworldControllerTests()
        {
            _mockCinemaworldService = new Mock<ICinemaworldService>();
            _mockFilmworldService = new Mock<IFilmworldService>();
            _mockLogger = new Mock<ILogger<MovieWorldController>>();
            _mockDistributedCache = new Mock<IDistributedCache>();
            _controller = new MovieWorldController(_mockFilmworldService.Object, _mockCinemaworldService.Object, _mockDistributedCache.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task Movies_NotFoundResult()
        {
            MoviesResponse? movies = null;

            _mockCinemaworldService.Setup(m => m.GetMoviesAsync()).ReturnsAsync(movies);
            var actual = await _controller.GetCWMoviesAsync();
            
            Assert.Equal(actual.Value, movies);
            Assert.IsType<NotFoundResult>(actual.Result);
        }

        [Fact]
        public async Task Movies_OkResult()
        {
            MoviesResponse? moviesResponse = new MoviesResponse();
            moviesResponse.Movies = new List<Movie>() { new Movie() { ID = "fw00001"} };
            _mockFilmworldService.Setup(m => m.GetMoviesAsync()).ReturnsAsync(moviesResponse);
            var actual = await _controller.GetFWMoviesAsync();

            Assert.NotNull(actual);
            Assert.IsType<OkObjectResult>(actual.Result);
        }

    }
}