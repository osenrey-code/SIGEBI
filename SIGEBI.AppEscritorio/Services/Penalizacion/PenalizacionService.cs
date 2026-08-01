using SIGEBI.AppEscritorio.Dtos.Penalizaciones;
using SIGEBI.AppEscritorio.Services.Api;


namespace SIGEBI.AppEscritorio.Services.Penalizaciones
{
    public class PenalizacionService : IPenalizacionService
    {
        private readonly IApiClient _apiClient;

        public PenalizacionService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<PenalizacionDto>> ConsultarPenalizacionesAsync(ConsultarPenalizacionesRequestDto request)
        {
            var queryParams = new List<string>();

            if (request.UsuarioId.HasValue && request.UsuarioId.Value > 0)
                queryParams.Add($"usuarioId={request.UsuarioId.Value}");

            if (request.PrestamoId.HasValue && request.PrestamoId.Value > 0)
                queryParams.Add($"prestamoId={request.PrestamoId.Value}");

            if (!string.IsNullOrWhiteSpace(request.Estado))
                queryParams.Add($"estado={Uri.EscapeDataString(request.Estado.Trim())}");

            string endpoint = "api/penalizaciones/consultar";
            if (queryParams.Count > 0)
                endpoint += "?" + string.Join("&", queryParams);

            var resultado = await _apiClient.GetTAsync<List<PenalizacionDto>>(endpoint);
            return resultado ?? new List<PenalizacionDto>();
        }

        public async Task ResolverPenalizacionAsync(ResolverPenalizacionRequestDto request)
        {
            await _apiClient.PatchAsync("api/penalizaciones/resolver", request);
        }
    }
}