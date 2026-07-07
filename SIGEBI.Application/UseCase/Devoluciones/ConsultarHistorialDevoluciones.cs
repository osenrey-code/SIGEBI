using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;


namespace SIGEBI.Application.UseCase.Devoluciones
{
    public class ConsultarHistorialDevoluciones
    {
        private readonly IRepositorioDevolucion _devoluciones;

        public ConsultarHistorialDevoluciones(IRepositorioDevolucion devolucion)
        {
            _devoluciones = devolucion;
        }

        public async Task<IEnumerable<DevolucionResponse>> EjecutarAsync(ConsultarHistorialDevolucionesRequest request)
        {
            var resultados = await _devoluciones.ConsultarHistorialAsync(
                request.UsuarioId,
                request.EjemplarId,
                request.FechaInicio,
                request.FechaFin
            );

            var historialResponse = resultados.Select(d =>
            {
                int diasRetraso = d.Prestamo != null ? d.Prestamo.CalcularDiasRetraso(d.FechaDevolucion) : 0;
                bool tieneDanios = d.MultaPorDanios();
                bool generoPenalizacion = diasRetraso > 0 || tieneDanios;

                return new DevolucionResponse
                {
                    PrestamoId = d.PrestamoId,

                    TituloRecurso = d.Prestamo?.Ejemplar?.RecursoBibliografico?.Titulo ?? "Recurso no especificado",
                    FechaDevolucion = d.FechaDevolucion,
                    DiasRetraso = diasRetraso,
                    Condicion = d.Condicion,
                    PenalizacionGenerada = generoPenalizacion,
                    MontoPenalizacion = 0,
                    Mensaje = "Registro de historial"
                };
            });

            return historialResponse.ToList();
        }
    }
}
