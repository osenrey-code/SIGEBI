using SIGEBI.Application.DTOs.Response;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Service
{
    public interface IAuditoriaService
    {
        Task RegistrarAsync(int UsuarioId, string Accion, string EntidadAfectada, string detalles = "");
        Task<IEnumerable<LogAuditoriaResponse>> ListarHistorialAsync(string? UsuarioId = null, string? EntidadAfectada = null);
    }
}
