using System.Net.Http.Headers;
using SIGEBI.AppEscritorio.Session;

namespace SIGEBI.AppEscritorio.Handlers
{
    public class JwtBearerHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (UserSession.Instancia.EstaAutenticado)
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", UserSession.Instancia.TokenJwt);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
