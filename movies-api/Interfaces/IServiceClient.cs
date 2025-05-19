using MoviesApi.Models;

namespace MoviesApi.Interfaces
{
    public interface IServiceClient
    {
        
        Task<T?> GetAsync<T>(string serviceName, string route, CancellationToken cancellationToken = default) where T : class;
    }
}
