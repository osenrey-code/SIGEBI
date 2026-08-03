using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace SIGEBI.AppWeb.Handlers
{
    public class JwtBearerHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccesor;

        public JwtBearerHandler(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccesor = httpContextAccesor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccesor.HttpContext;

            if (httpContext != null)
            {
                var token = await httpContext.GetTokenAsync("access_token");

                if (!string.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
