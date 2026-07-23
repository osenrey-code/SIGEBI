using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.Application.Interfaces.Service
{
    public interface IGestionPrestamos
    {
        Task<SolicitudResponse> SolicitarPrestamoAsync(
            RegistrarSolicitudRequest request,
            int usuarioId
        );

        Task<PrestamoResponse> AprobarPrestamoAsync(
            AprobarSolicitudRequest request,
            int usuarioEjecutorId
        );

        Task<IEnumerable<PrestamoResponse>> ConsultarHistorialAsync(
            ConsultarHistorialPrestamosRequest request
        );

        Task<IEnumerable<PrestamoResponse>> ConsultarPrestamosActivosAsync(
            ConsultarPrestamosActivosRequest request, int usuarioId
        );

        Task<IEnumerable<SolicitudResponse>> ConsultarTodasAsync();
        Task<IEnumerable<SolicitudResponse>> ConsultarSolicitudesPendientesAsync();
        Task<SolicitudResponse?> ObtenerPorIdConDetallesAsync(int id);

    }
}