using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MoviesApi.Extensions;
using MoviesApi.Interfaces;
using MoviesApi.Models;
using Serilog;
using System.Collections.Generic;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieWorldController : ControllerBase, IMovieWorldController
    {
        public readonly IFilmworldService _filmworldService;
        public readonly ICinemaworldService _cinemaworldService;
        public readonly ILogger<MovieWorldController> _logger;
        public MovieWorldController(IFilmworldService filmworldService,
            ICinemaworldService cinemaworldService,
            IDistributedCache distributedCache
            , ILogger<MovieWorldController> logger
            )
        {
            _filmworldService = filmworldService;
            _cinemaworldService = cinemaworldService;
            _logger = logger;
        }
        [HttpGet, Route("get-filmworld-movies")]
        public async Task<ActionResult<MoviesResponse>> GetFWMoviesAsync()
        {
            _logger.LogInformation("FilmworldController-GetMovies Start");

            var movies = await _filmworldService.GetMoviesAsync();
            if (movies == null) return NotFound();

            return Ok(movies);
        }

        [HttpGet, Route("get-cinemaworld-movies")]
        public async Task<ActionResult<MoviesResponse>> GetCWMoviesAsync()
        {
            _logger.LogInformation("FilmworldController-GetMovies Start");

            var movies = await _filmworldService.GetMoviesAsync();
            if (movies == null) return NotFound();

            return Ok(movies);
        }

        [HttpGet, Route("get-movie-by-id")]
        public async Task<ActionResult<MovieDetails>> GetMovieByIdAsync(string movieId)
        {
            _logger.LogInformation("FilmworldController-GetMovieByIdAsync Start");
            if (movieId == null || movieId.Length == 0 || movieId.Length > 10)
            {
                return BadRequest("Invalid movie Id");
            }
            MovieDetails? movieDetails = null;
            if (movieId.Substring(0, 2) == "fw")
                movieDetails = await _filmworldService.GetMovieByIdAsync(movieId);
            else if (movieId.Substring(0, 2) == "cw")
                movieDetails = await _cinemaworldService.GetMovieByIdAsync(movieId);

            if (movieDetails == null)
                return NotFound();

            return Ok(movieDetails);
        }

        [HttpGet, Route("get-best-priced-movies")]
        public async Task<ActionResult<IList<MovieDetails>>> GetBestPricedMoviesAsync()
        {
            _logger.LogInformation("FilmworldController-GetMovies Start");
            try
            {
                var fwMovies = await _filmworldService.GetMoviesAsync();
                var cwMovies = await _cinemaworldService.GetMoviesAsync();
                if (fwMovies == null && cwMovies == null) return NotFound();

                var loadPosts = new List<Task<MovieDetails>>();
                if (fwMovies != null)
                {
                    foreach (var fwMovie in fwMovies.Movies)
                    {
                        var movieDetails = _filmworldService.GetMovieByIdAsync(fwMovie.ID);
                        loadPosts.Add(movieDetails);
                    }
                }
                if (cwMovies != null)
                {
                    foreach (var cwMovie in cwMovies.Movies)
                    {
                        var movieDetails = _cinemaworldService.GetMovieByIdAsync(cwMovie.ID);
                        loadPosts.Add(movieDetails);
                    }
                }
                var response = await Task.WhenAll(loadPosts);
                var groups = response.GroupBy(g => g?.Title);
                var min = groups.Min(g => g.Count());

                var items = groups.Select(g => g.OrderBy(o => double.Parse(o?.Price)).FirstOrDefault()).ToList();

                return Ok(items);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return NotFound("Please try after sometime");
            }
        }
    }
}

