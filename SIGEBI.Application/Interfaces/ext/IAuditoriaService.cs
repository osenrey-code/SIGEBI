
using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.Application.Interfaces.ext
{
    public interface IAuditoriaService
    {
        Task RegistrarAsync(int UsuarioId, string Accion, string EntidadAfectada, string detalles = "");
        Task<IEnumerable<LogAuditoriaResponse>> ListarHistorialAsync(string? UsuarioId = null, string? EntidadAfectada = null);
    }
}
