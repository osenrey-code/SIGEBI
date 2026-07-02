using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class ConsultarCatalogo
    {
        private readonly IRepositorioRecurso _recursos;

        public ConsultarCatalogo(IRepositorioRecurso recursos)
        {
            _recursos = recursos;
        }

        public async Task<IEnumerable<RecursoResponse>> ConsultarTodosAsync()
        {
            var libros = await _recursos.ObtenerTodosAsync();
            return libros.Select(l => new RecursoResponse
            {
                ISBN = l.ISBN,
                Titulo = l.Titulo,
                Autor = l.Autor,
                NumeroEjemplares = l.NumeroEjemplares,
                Categoria = "N/A"
            });
        }


    }
}