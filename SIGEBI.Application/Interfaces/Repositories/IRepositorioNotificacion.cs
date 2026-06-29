using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioNotificacion : ReadOnly<Notificacion>, Writer<Notificacion>
    {
        Task<IEnumerable<Notificacion>> ConsultarAsync(
            Guid? usuarioDestinatarioId,
            string? tipoEvento,
            DateTime? fechaInicio,
            DateTime? fechaFin);
    }
}