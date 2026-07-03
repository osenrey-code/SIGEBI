using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioAuditoria : IRepositoryInmutable<Auditoria>
    {
        Task<IEnumerable<Auditoria>> ObtenerPorEjecutorAsync(int usuarioId);

        Task<IEnumerable<Auditoria>> ObtenerPorEntidadAsync(string entidad);

        Task<IEnumerable<Auditoria>> ConsultarAsync(
            int? usuarioId,
            string? accion,
            string? entidadAfectada,
            DateTime? fechaInicio,
            DateTime? fechaFin
        );
    }
}