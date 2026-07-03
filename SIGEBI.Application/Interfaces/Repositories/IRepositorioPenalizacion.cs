using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioPenalizacion : IBaseRepository<Penalizacion>
    {
        Task<IEnumerable<Penalizacion>> ObtenerPorUsuarioAsync(int usuarioId);

        Task<IEnumerable<Penalizacion>> ObtenerActivasPorUsuarioAsync(int usuarioId);

        Task<bool> ExisteActivaPorUsuarioAsync(int usuarioId);

        Task<IEnumerable<Penalizacion>> ConsultarAsync(
            int? usuarioId,
            int? prestamoId,
            string? estado
        );
    }
}