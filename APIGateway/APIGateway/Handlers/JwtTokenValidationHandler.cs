using System.Net.Http.Headers;

namespace APIGateway.Handlers
{
    public class JwtTokenValidationHandler : DelegatingHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public JwtTokenValidationHandler(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.PathAndQuery.StartsWith("/api/Auth"))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var token = request.Headers.Authorization?.Parameter;

            if (string.IsNullOrEmpty(token))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            var validationRequest = new HttpRequestMessage(HttpMethod.Get, _configuration["JwtSettings:UrlProtected"]);

            validationRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var validationResponse = await _httpClient.SendAsync(validationRequest, cancellationToken);

            if (validationResponse.IsSuccessStatusCode)
            {
                return await base.SendAsync(request, cancellationToken);
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
        }


    }
}
