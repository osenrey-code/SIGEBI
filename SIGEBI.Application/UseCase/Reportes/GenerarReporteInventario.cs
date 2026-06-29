using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Enums;


namespace SIGEBI.Application.UseCase.Reportes
{
    public class GenerarReporteInventario
    {
        private readonly IRepositorioRecurso _recursos;

        public GenerarReporteInventario(IRepositorioRecurso recursos)
        {
            _recursos = recursos;
        }

        public async Task<ResultadoOperacionResponse<ReporteInventarioResponse>> EjecutarAsync()
        {
            var recursos = await _recursos.ObtenerTodosAsync();

            var lista = recursos.ToList();

            var response = new ReporteInventarioResponse
            {
                TotalRecursos = lista.Count,

                RecursosDisponibles = lista.Count(r =>
                    r.Estado == EstadoRecurso.Disponible
                ),

                RecursosPrestados = lista.Count(r =>
                    r.Estado == EstadoRecurso.Prestado
                ),

                RecursosReservados = lista.Count(r =>
                    r.Estado == EstadoRecurso.Reservado
                ),

                RecursosFueraDeServicio = lista.Count(r =>
                    r.Estado == EstadoRecurso.FueraDeServicio
                )
            };

            return ResultadoOperacionResponse<ReporteInventarioResponse>.Ok(
                "Reporte de inventario generado correctamente.",
                response
            );
        }
    }
}
