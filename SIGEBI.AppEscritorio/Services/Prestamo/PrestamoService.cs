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
            return await _apiClient.GetTAsync<List<PrestamoDto>>("api/prestamos/consultar/activos") ?? new List<PrestamoDto>();
        }

        public async Task<List<PrestamoDto>> ConsultarHistorialAsync(ConsultarHistorialPrestamosRequest request)
        {
            return await _apiClient.GetTAsync<List<PrestamoDto>>("api/prestamos/historial") ?? new List<PrestamoDto>();
        }
    }
}
