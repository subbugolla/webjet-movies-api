using Microsoft.AspNetCore.Mvc;
using MoviesApi.Models;

namespace MoviesApi.Interfaces
{
    public interface IMovieWorldController
    {
        Task<ActionResult<MoviesResponse>> GetFWMoviesAsync();
        Task<ActionResult<MoviesResponse>> GetCWMoviesAsync();
        Task<ActionResult<MovieDetails>> GetMovieByIdAsync(string movieId);
        Task<ActionResult<IList<MovieDetails>>> GetBestPricedMoviesAsync();
    }
}
