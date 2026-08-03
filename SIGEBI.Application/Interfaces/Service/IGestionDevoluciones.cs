using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.Application.Interfaces.Service
{
    public interface IGestionDevolucionesUseCase
    {
        Task<DevolucionResponse> RegistrarDevolucionAsync(
            RegistrarDevolucionRequest request,
            int bibliotecarioId
        );

        Task<IEnumerable<DevolucionResponse>> ConsultarHistorialAsync(
            ConsultarHistorialDevolucionesRequest request
        );
    }
}