using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioAuditoria : IRepositoryInmutable<Auditoria>
    {
        Task<IEnumerable<Auditoria>> ObtenerPorEjecutorAsync(int UsuarioId);
        Task<IEnumerable<Auditoria>> ObtenerPorEntidadAsync(string entidad);
    }
}