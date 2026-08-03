using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioNotificacion : IBaseRepository<Notificacion>
    {
        Task<IEnumerable<Notificacion>> ObtenerPorUsuarioAsync(int usuarioId);

        Task<IEnumerable<Notificacion>> ObtenerNoLeidasPorUsuarioAsync(int usuarioId);

        Task<IEnumerable<Notificacion>> ObtenerTodoElHistorialAsync();
    }
}