using SIGEBI.Application.DTOs.Response;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioPenalizacion : IBaseRepository<Penalizacion>
    {
        Task<IEnumerable<Penalizacion>> ObtenerPorUsuarioAsync(int usuarioId);

        Task<Penalizacion?> ObtenerActivaPorUsuarioAsync(int usuarioId);

        Task<bool> TienePenalizacionActivaAsync(int usuarioId);

        Task<IEnumerable<Penalizacion>> ConsultarAsync(
            int? usuarioId,
            EstadoPenalizacion? estado,
            DateTime? fechaInicio,
            DateTime? fechaFin
        );

        Task<ReportePenalizacionesResponse> ObtenerEstadisticaPenalizacionesAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}