using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.Application.Interfaces.Service;

public interface IGestionPenalizaciones
{
    Task<IEnumerable<PenalizacionResponse>> ConsultarPenalizacionesAsync(
        ConsultarPenalizacionesRequest request,
        int usuarioId
    );

    Task<IEnumerable<PenalizacionResponse>> ConsultarPenalizacionesActivasAsync(
        ConsultarPenalizacionesActivasRequest request,
        int usuarioId
    );

    Task<PenalizacionResponse> ResolverPenalizacionAsync(
        ResolverPenalizacionRequest request,
        int usuarioId
    );
}