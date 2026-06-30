using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioRecurso : IBaseRepository<RecursoBibliografico>
    {
        Task<RecursoBibliografico?> ObtenerPorIdentificadorAsync(string identificador);

        Task<IEnumerable<RecursoBibliografico>> ConsultarCatalogoAsync(
            string? titulo,
            string? autor,
            string? categoria,
            bool? soloDisponibles
            );
    }
}