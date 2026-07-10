using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class ConsultarDetalleRecurso
    {
        private readonly IRepositorioRecurso _recursos;

        public ConsultarDetalleRecurso(IRepositorioRecurso recursos)
        {
            _recursos = recursos;
        }

        public async Task<RecursoResponse> EjecutarAsync(
            ConsultarDetalleRecursoRequest request)
        {
            Guard.NotNull(request, "Los datos de consulta del recurso");

            if (request.RecursoBibliograficoId <= 0)
                throw new BusinessException("El recurso es obligatorio.");

            var recurso = await _recursos.BuscarConCategoriaAsync(
                request.RecursoBibliograficoId
            );

            if (recurso is null)
                throw new BusinessException("El recurso no existe.");

            return MapearRecurso(recurso);
        }

        private static RecursoResponse MapearRecurso(RecursoBibliografico recurso)
        {
            return new RecursoResponse
            {
                RecursoBibliograficoId = recurso.RecursoBibliograficoId,
                ISBN = recurso.ISBN,
                Titulo = recurso.Titulo,
                Autor = recurso.Autor,
                CategoriaId = recurso.CategoriaId,
                Categoria = recurso.Categoria?.Nombre ?? "N/A",
                AnioPublicado = recurso.AnioPublicado,
                ImagenUrl = recurso.ImagenUrl,
                TotalEjemplares = recurso.TotalEjemplares,
                CopiasDisponibles = recurso.CopiasDisponibles
            };
        }
    }
}