using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Reportes
{
     public class GenerarReportesUsoCatalogo
    {
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IRepositorioRecurso _recursos;

        public GenerarReportesUsoCatalogo(
            IRepositorioPrestamo prestamos,
            IRepositorioRecurso recursos)
        {
            _prestamos = prestamos;
            _recursos = recursos;
        }

        public async Task<ResultadoOperacionResponse<ReporteUsoCatalogoResponse>> EjecutarAsync(
            GenerarReporteRequest request)
        {
            if (request.FechaInicio.HasValue &&
                request.FechaFin.HasValue &&
                request.FechaInicio.Value.Date > request.FechaFin.Value.Date)
            {
                return ResultadoOperacionResponse<ReporteUsoCatalogoResponse>.Error(
                    "La fecha de inicio no puede ser mayor que la fecha final."
                );
            }

            var prestamos = await _prestamos.ObtenerTodosAsync();
            var recursos = await _recursos.ObtenerTodosAsync();

            if (request.FechaInicio.HasValue)
            {
                prestamos = prestamos.Where(p =>
                    p.FechaSolicitud.Date >= request.FechaInicio.Value.Date
                );
            }

            if (request.FechaFin.HasValue)
            {
                prestamos = prestamos.Where(p =>
                    p.FechaSolicitud.Date <= request.FechaFin.Value.Date
                );
            }

            var listaPrestamos = prestamos.ToList();
            var listaRecursos = recursos.ToList();

            var prestamosConRecurso = listaPrestamos
                .Join(
                    listaRecursos,
                    prestamo => prestamo.RecursoId,
                    recurso => recurso.Id,
                    (prestamo, recurso) => new
                    {
                        Prestamo = prestamo,
                        Recurso = recurso
                    }
                )
                .ToList();

            var recursosMasSolicitados = prestamosConRecurso
                .GroupBy(x => new
                {
                    x.Recurso.Id,
                    x.Recurso.Titulo
                })
                .Select(g => new RecursoSolicitadoReporteResponse
                {
                    RecursoId = g.Key.Id,
                    Titulo = g.Key.Titulo,
                    CantidadSolicitudes = g.Count()
                })
                .OrderByDescending(x => x.CantidadSolicitudes)
                .Take(10)
                .ToList();

            var demandaPorCategoria = prestamosConRecurso
                .GroupBy(x => x.Recurso.Categoria)
                .Select(g => new DemandaCategoriaReporteResponse
                {
                    Categoria = string.IsNullOrWhiteSpace(g.Key)
                        ? "Sin categoría"
                        : g.Key,
                    CantidadSolicitudes = g.Count()
                })
                .OrderByDescending(x => x.CantidadSolicitudes)
                .ToList();

            var recursosDisponibles = listaRecursos.Count(r =>
                r.Estado == EstadoRecurso.Disponible
            );

            var porcentajeDisponibilidad = listaRecursos.Count == 0
                ? 0
                : Math.Round(
                    (decimal)recursosDisponibles / listaRecursos.Count * 100,
                    2
                );

            var response = new ReporteUsoCatalogoResponse
            {
                TotalSolicitudes = listaPrestamos.Count,
                RecursosDiferentesSolicitados = listaPrestamos
                    .Select(p => p.RecursoId)
                    .Distinct()
                    .Count(),

                PorcentajeDisponibilidadActual = porcentajeDisponibilidad,
                RecursosMasSolicitados = recursosMasSolicitados,
                DemandaPorCategoria = demandaPorCategoria
            };

            return ResultadoOperacionResponse<ReporteUsoCatalogoResponse>.Ok(
                "Reporte de uso del catálogo generado correctamente.",
                response
            );
        }
    }
}
