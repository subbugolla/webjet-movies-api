using MoviesApi.Common;
using MoviesApi.Interfaces;
using Newtonsoft.Json;

namespace MoviesApi.Services
{
    public class ServiceClient : IServiceClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<T?> GetAsync<T>(string serviceName, string route, CancellationToken cancellationToken = default) where T : class
        {
            var _httpClient = _httpClientFactory.CreateClient(serviceName);
            var response = await _httpClient.GetAsync(route, cancellationToken);
            
            if (response != null && response.IsSuccessStatusCode)
            {
                var result = response.Content?.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T?>(result);
            }
            return null;
        }
    }
}
