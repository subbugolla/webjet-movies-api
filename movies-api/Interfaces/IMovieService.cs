using MoviesApi.Models;

namespace MoviesApi.Interfaces
{
    public interface IMovieService
    {
        Task<MoviesResponse?> GetMoviesAsync();
        Task<MovieDetails?> GetMovieByIdAsync(string id);
    }
}
