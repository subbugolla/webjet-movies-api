using MoviesApi.Common;
using System.Net;

namespace MoviesApi.Middleware
{
    public class HeaderMiddleware : DelegatingHandler
    {
        private readonly IConfiguration _configuration;
        public HeaderMiddleware(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(ApiConstants.X_Access_Token))
            {
                request.Headers.Add(ApiConstants.X_Access_Token, _configuration[ApiConstants.X_Access_Token]);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
