using Microsoft.Extensions.Caching.Distributed;
using MoviesApi.Common;
using MoviesApi.Extensions;
using MoviesApi.Interfaces;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public class CinemaworldService : ICinemaworldService
    {
        private readonly IServiceClient _client;
        private readonly IDistributedCache _cache;
        private const string movieworldMoviesListCacheKey = "movieworldMoviesList";
        public CinemaworldService(IServiceClient serviceClient, IDistributedCache distributedCache)
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
                    movieDetails = await _client.GetAsync<MovieDetails>(ApiConstants.CinemaworldApiUrl, string.Format(ApiConstants.Get_Cinemaworld_MovieById_Route, Id));
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
            if (_cache.TryGetValue(movieworldMoviesListCacheKey, out MoviesResponse? moviesResponse))
            {
                return moviesResponse;
            }
            else
            {
                movies = await _client.GetAsync<MoviesResponse>(ApiConstants.CinemaworldApiUrl, ApiConstants.Get_Cinemaworld_All_Movies_Route);
                if (movies == null)
                    return null;

                await _cache.SetAsync(movieworldMoviesListCacheKey, movies, GetDistributedCacheEntryOptions());
            }
            return movies;
        }

        private DistributedCacheEntryOptions GetDistributedCacheEntryOptions() => new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(600)).SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));

    }
}
