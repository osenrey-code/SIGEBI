using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioRecurso : IBaseRepository<RecursoBibliografico>
    {
        Task<RecursoBibliografico?> BuscarPorIsbnAsync(string isbn);
        Task<RecursoBibliografico?> BuscarConCategoria(string isbn);

        Task<IEnumerable<RecursoBibliografico>> ConsultarCatalogoAsync(
            string? titulo,
            string? autor,
            string? categoria,
            bool? soloDisponibles
            );
    }
}