using Microsoft.Extensions.Caching.Distributed;
using MoviesApi.Common;
using MoviesApi.Extensions;
using MoviesApi.Interfaces;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public class FilmworldService : IFilmworldService
    {
        private readonly IServiceClient _client;
        private readonly IDistributedCache _cache;
        private const string filmworldMoviesListCacheKey = "filmworldMoviesList";

        public FilmworldService(IServiceClient serviceClient, IDistributedCache distributedCache)
        {
            _client = serviceClient;
            _cache = distributedCache;
        }
        public async Task<MovieDetails?> GetMovieByIdAsync(string Id)
        {
            MovieDetails? movieDetails = null;

            if (!string.IsNullOrEmpty(Id))
            {
                if (_cache.TryGetValue(Id, out movieDetails))
                {
                    return movieDetails;
                }
                else
                {
                    movieDetails  = await _client.GetAsync<MovieDetails>(ApiConstants.FilmworldApiUrl, string.Format(ApiConstants.Get_Filmworld_MovieById_Route, Id));
                    if (movieDetails == null)
                        return null;

                    await _cache.SetAsync(movieDetails.ID, movieDetails, GetDistributedCacheEntryOptions());
                }
                    
            }
            return movieDetails;
        }

        public async Task<MoviesResponse?> GetMoviesAsync()
        {
            MoviesResponse? movies = null;
            if (_cache.TryGetValue(filmworldMoviesListCacheKey, out MoviesResponse? moviesResponse))
            {
                return moviesResponse;
            }
            else
            {
                movies =  await _client.GetAsync<MoviesResponse>(ApiConstants.FilmworldApiUrl, ApiConstants.Get_Filmworld_All_Movies_Route);
                if (movies == null) 
                    return null;

                await _cache.SetAsync(filmworldMoviesListCacheKey, movies, GetDistributedCacheEntryOptions());
            }
            return movies;
        }

        private DistributedCacheEntryOptions GetDistributedCacheEntryOptions() => new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(600)).SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
    }
    
}
