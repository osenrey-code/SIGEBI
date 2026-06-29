using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioAuditoria : ReadOnly<RegistroAuditoria>, Writer<RegistroAuditoria>
    {
        Task<IEnumerable<RegistroAuditoria>> ConsultarAsync(
            Guid? usuarioId,
            string? accion,
            string? entidadAfectada,
            DateTime? fechaInicio,
            DateTime? fechaFin);
    }
}