using SIGEBI.AppEscritorio.Dtos.Auditorias;
using SIGEBI.AppEscritorio.Services.Api;

namespace SIGEBI.AppEscritorio.Services.Auditoria
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly IApiClient _apiClient;
        private const string BaseUrl = "api/auditoria";

        public AuditoriaService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<LogAuditoriaResponseDto>> ConsultarLogsAsync(ConsultarLogAuditoriaRequestDto request)
        {
            var queryParams = new List<string>();

            if (request.UsuarioId.HasValue && request.UsuarioId > 0)
            {
                queryParams.Add($"usuarioId={request.UsuarioId.Value}");
            }

            if (!string.IsNullOrWhiteSpace(request.Accion))
            {
                queryParams.Add($"accion={Uri.EscapeDataString(request.Accion.Trim())}");
            }

            if (!string.IsNullOrWhiteSpace(request.EntidadAfectada))
            {
                queryParams.Add($"entidadAfectada={Uri.EscapeDataString(request.EntidadAfectada.Trim())}");
            }

            if (request.FechaInicio.HasValue)
            {
                queryParams.Add($"fechaInicio={request.FechaInicio.Value:yyyy-MM-ddTHH:mm:ss}");
            }

            if (request.FechaFin.HasValue)
            {
                queryParams.Add($"fechaFin={request.FechaFin.Value:yyyy-MM-ddTHH:mm:ss}");
            }

            string url = queryParams.Any()
                ? $"{BaseUrl}/consultar?{string.Join("&", queryParams)}"
                : $"{BaseUrl}/consultar";

            var resultado = await _apiClient.GetTAsync<List<LogAuditoriaResponseDto>>(url);
            return resultado ?? new List<LogAuditoriaResponseDto>();
        }
    }
}