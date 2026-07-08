using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Devoluciones
{
    public class ConsultarHistorialDevoluciones
    {
        private readonly IRepositorioDevolucion _devoluciones;

        public ConsultarHistorialDevoluciones(IRepositorioDevolucion devoluciones)
        {
            _devoluciones = devoluciones;
        }

        public async Task<IEnumerable<DevolucionResponse>> EjecutarAsync(
            ConsultarHistorialDevolucionesRequest request)
        {
            Guard.NotNull(request, "Los filtros del historial de devoluciones");

            if (request.UsuarioId.HasValue && request.UsuarioId.Value <= 0)
                throw new BusinessException("El usuario no existe.");

            if (request.RecursoBibliograficoId.HasValue && request.RecursoBibliograficoId.Value <= 0)
                throw new BusinessException("El recurso bibliográfico no existe.");

            if (request.EjemplarId.HasValue && request.EjemplarId.Value <= 0)
                throw new BusinessException("El ejemplar debe ser mayor que cero.");

            if (request.FechaInicio.HasValue &&
                request.FechaFin.HasValue &&
                request.FechaInicio.Value > request.FechaFin.Value)
            {
                throw new BusinessException("La fecha de inicio no puede ser mayor que la fecha final.");
            }

            var resultados = await _devoluciones.ConsultarHistorialAsync(
                request.UsuarioId,
                request.RecursoBibliograficoId,
                request.EjemplarId,
                request.FechaInicio,
                request.FechaFin
            );

            return resultados
                .Select(devolucion =>
                {
                    int diasRetraso = devolucion.Prestamo is not null
                        ? devolucion.Prestamo.CalcularDiasRetraso(devolucion.FechaDevolucion)
                        : 0;

                    decimal montoPenalizacion = diasRetraso > 0
                        ? diasRetraso * 25m
                        : 0;

                    string tituloRecurso = devolucion.Prestamo?.Ejemplar?.RecursoBibliografico?.Titulo
                        ?? "Recurso no especificado";

                    return new DevolucionResponse
                    {
                        PrestamoId = devolucion.PrestamoId,
                        TituloRecurso = tituloRecurso,
                        FechaDevolucion = devolucion.FechaDevolucion,
                        DiasRetraso = diasRetraso,
                        Condicion = devolucion.Condicion,
                        PenalizacionGenerada = diasRetraso > 0,
                        MontoPenalizacion = montoPenalizacion,
                        Mensaje = diasRetraso > 0
                            ? $"Devolución tardía con {diasRetraso} día(s) de retraso."
                            : "Devolución realizada dentro del plazo."
                    };
                })
                .ToList();
        }
    }
}