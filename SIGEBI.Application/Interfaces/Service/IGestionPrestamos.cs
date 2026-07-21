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
            ConsultarPrestamosActivosRequest request
        );
    }
}