using SIGEBI.AppEscritorio.Dtos.Prestamos;
using SIGEBI.AppEscritorio.Services.Api;


namespace SIGEBI.AppEscritorio.Services.Prestamo
{
    public class PrestamoService : IPrestamoService
    {
        private readonly IApiClient _apiClient;

        public PrestamoService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<SolicitudDto>> ConsultarTodasSolicitudesAsync()
        {
            return await _apiClient.GetTAsync<List<SolicitudDto>>("api/solicitudes") ?? new List<SolicitudDto>();
        }

        public async Task<List<SolicitudDto>> ConsultarSolicitudesPendientesAsync()
        {
            return await _apiClient.GetTAsync<List<SolicitudDto>>("api/solicitudes/pendientes") ?? new List<SolicitudDto>();
        }

        public async Task<SolicitudDto?> ObtenerDetalleSolicitudAsync(int id)
        {
            return await _apiClient.GetTAsync<SolicitudDto>($"api/solicitudes/{id}");
        }

        public async Task AprobarSolicitudAsync(AprobarSolicitudRequest request)
        {
            await _apiClient.PostAsync<AprobarSolicitudRequest, object>("api/solicitudes/aprobar", request);
        }

        public async Task<List<PrestamoDto>> ConsultarActivosAsync(ConsultarPrestamosActivosRequest request)
        {
            var queryParams = new List<string>();

            if (request != null)
            {
                if (!string.IsNullOrWhiteSpace(request.Identificacion))
                {
                    queryParams.Add($"identificacion={Uri.EscapeDataString(request.Identificacion.Trim())}");
                }
            }

            string url = queryParams.Any()
                ? $"api/prestamos/consultar/activos?{string.Join("&", queryParams)}"
                : "api/prestamos/consultar/activos";

            return await _apiClient.GetTAsync<List<PrestamoDto>>(url) ?? new List<PrestamoDto>();
        }

        public async Task<List<PrestamoDto>> ConsultarHistorialAsync(ConsultarHistorialPrestamosRequest request)
        {
            var queryParams = new List<string>();

            if (request != null)
            {
                if (!string.IsNullOrWhiteSpace(request.Identificacion))
                {
                    queryParams.Add($"identificacion={Uri.EscapeDataString(request.Identificacion.Trim())}");
                }

                if (request.RecursoBibliograficoId.HasValue && request.RecursoBibliograficoId.Value > 0)
                {
                    queryParams.Add($"recursoBibliograficoId={request.RecursoBibliograficoId.Value}");
                }

                if (request.EjemplarId.HasValue && request.EjemplarId.Value > 0)
                {
                    queryParams.Add($"ejemplarId={request.EjemplarId.Value}");
                }
            }

            string url = queryParams.Any()
                ? $"api/prestamos/historial?{string.Join("&", queryParams)}"
                : "api/prestamos/historial";

            return await _apiClient.GetTAsync<List<PrestamoDto>>(url) ?? new List<PrestamoDto>();
        }

        public async Task RechazarSolicitudAsync(RechazarSolicitudRequest request)
        {
            await _apiClient.PostAsync<RechazarSolicitudRequest, object>("api/solicitudes/rechazar", request);
        }
    }
}
