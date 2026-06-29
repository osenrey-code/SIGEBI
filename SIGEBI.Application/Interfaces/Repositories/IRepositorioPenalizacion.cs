using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioPenalizacion : ReadOnly<Penalizacion>, Writer<Penalizacion>
    {
        Task<IEnumerable<Penalizacion>> ObtenerPorPerfilLectorAsync(Guid perfilLectorId);

        Task<Penalizacion?> ObtenerActivaPorPerfilLectorAsync(Guid perfilLectorId);

        Task<bool> ExisteActivaPorPerfilLectorAsync(Guid perfilLectorId);

        Task<IEnumerable<Penalizacion>> ConsultarAsync(
            Guid? usuarioId,
            Guid? perfilLectorId,
            EstadoPenalizacion? estado,
            DateTime? fechaInicio,
            DateTime? fechaFin);
    }
}