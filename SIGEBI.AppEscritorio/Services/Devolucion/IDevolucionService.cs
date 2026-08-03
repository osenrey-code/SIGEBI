
using SIGEBI.AppEscritorio.Dtos.Devoluciones;

namespace SIGEBI.AppEscritorio.Services.Devolucion
{
    public interface IDevolucionService
    {
        Task<DevolucionResponseDto?> RegistrarDevolucionAsync(RegistrarDevolucionRequestDto request);
        Task<IEnumerable<DevolucionResponseDto>?> ConsultarHistorialAsync(ConsultarHistorialDevolucionesRequestDto request);
    }
}
