using SIGEBI.AppEscritorio.Dtos.Devoluciones;
using SIGEBI.AppEscritorio.Services.Api;

namespace SIGEBI.AppEscritorio.Services.Devolucion
{
    public class DevolucionService : IDevolucionService
    {
        private readonly IApiClient _apiClient;

        public DevolucionService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<DevolucionResponseDto?> RegistrarDevolucionAsync(RegistrarDevolucionRequestDto request)
        {
            return await _apiClient.PostAsync<RegistrarDevolucionRequestDto, DevolucionResponseDto>("api/devolucion/registrar", request);
        }

        public async Task<IEnumerable<DevolucionResponseDto>?> ConsultarHistorialAsync(ConsultarHistorialDevolucionesRequestDto request)
        {
            string url = "api/devolucion/historial?";
            var queryParams = new List<string>();

            if (request.UsuarioId.HasValue) queryParams.Add($"UsuarioId={request.UsuarioId.Value}");
            if (request.RecursoBibliograficoId.HasValue) queryParams.Add($"RecursoBibliograficoId={request.RecursoBibliograficoId.Value}");
            if (request.EjemplarId.HasValue) queryParams.Add($"EjemplarId={request.EjemplarId.Value}");
            if (request.FechaInicio.HasValue) queryParams.Add($"FechaInicio={request.FechaInicio.Value:yyyy-MM-dd}");
            if (request.FechaFin.HasValue) queryParams.Add($"FechaFin={request.FechaFin.Value:yyyy-MM-dd}");

            url += string.Join("&", queryParams);

            return await _apiClient.GetTAsync<IEnumerable<DevolucionResponseDto>>(url);
        }
    }
}