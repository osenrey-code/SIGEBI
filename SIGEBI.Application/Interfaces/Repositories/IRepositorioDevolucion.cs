using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioDevolucion
    {
        Task AgregarAsync(Devolucion devolucion);
        Task<Devolucion?> ObtenerPorIdAsync(int devolucionId);
        Task<Devolucion?> ObtenerPorPrestamoIdAsync(int prestamoId);
        Task<IEnumerable<Devolucion>> ConsultarHistorialAsync(
            int? usuarioId,
            int? recursoBibliograficoId,
            int? ejemplarId,
            DateTime? fechaInicio,
            DateTime? fechaFin
        );
    }
}
