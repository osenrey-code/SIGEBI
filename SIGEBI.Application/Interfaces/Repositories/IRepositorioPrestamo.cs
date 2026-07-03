using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioPrestamo : IBaseRepository<Prestamo>
    {
        Task<IEnumerable<Prestamo>> ObtenerActivosPorUsuarioAsync(int usuarioId);

        Task<IEnumerable<Prestamo>> ObtenerHistorialPorUsuarioAsync(int usuarioId);

        Task<Prestamo?> ObtenerConDetalleAsync(int prestamoId);
    }
}