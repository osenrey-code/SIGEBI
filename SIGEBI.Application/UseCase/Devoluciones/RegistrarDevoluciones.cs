using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;
using SIGEBI.Application.Interfaces.ext;

namespace SIGEBI.Application.UseCase.Devoluciones
{
    public class RegistrarDevoluciones
    {

        private const decimal MontoMoraPorDia = 25m;

        private readonly IRepositorioPrestamo _prestamos;
        private readonly IEjemplarRepository _ejemplares;
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IUsuario _usuarios;
        private readonly IRepositorioDevolucion _devoluciones;
        private readonly IAuditoriaService _auditoria;

        public RegistrarDevoluciones(IRepositorioPrestamo prestamos, IRepositorioPenalizacion penalizaciones,
            IUsuario usuarios, IRepositorioDevolucion devoluciones, IEjemplarRepository ejemplares, IAuditoriaService auditoria
  )
        {
            _prestamos = prestamos;
            _ejemplares = ejemplares;
            _penalizaciones = penalizaciones;
            _usuarios = usuarios;
            _devoluciones = devoluciones;
            _auditoria = auditoria;
      
        }

        public async Task<DevolucionResponse> EjecutarAsync(RegistrarDevolucionRequest request, int bibliotecarioId)
        {
            var prestamo = await _prestamos.ObtenerConDetallesAsync(request.PrestamoId);
            if (prestamo == null) throw new BusinessException("El préstamo especificado no existe.");

            if (prestamo.Ejemplar == null) throw new BusinessException("El ejemplar físico no está disponible.");
            prestamo.MarcarComoDevuelto();

            var nuevaDevolucion = new Devolucion(
                prestamoId: prestamo.PrestamoId,
                bibliotecarioId: bibliotecarioId,
                condicion: request.Condicion,
                observacion: request.Observacion
            );

            int diasRetraso = prestamo.CalcularDiasRetraso(nuevaDevolucion.FechaDevolucion);
            bool tieneDanios = nuevaDevolucion.MultaPorDanios();
            bool generoPenalizacion = diasRetraso > 0 || tieneDanios;

            prestamo.Ejemplar.RegistrarDevolucion(request.Observacion);

            if (tieneDanios)
            {
                prestamo.Ejemplar.MarcarFueraDeServicio($"Retirado por condición: {request.Condicion}. {request.Observacion}");
            }

            string mensajePenalizacion = string.Empty;
            decimal montoTotal = 0;

            if (generoPenalizacion)
            {
                var motivos = new List<string>();

                if (diasRetraso > 0)
                {
                    decimal multaPorRetraso = diasRetraso * 50.0m;
                    motivos.Add($"Retraso de {diasRetraso} días ({multaPorRetraso})");
                    montoTotal += multaPorRetraso;
                }

                if (tieneDanios)
                {
                    decimal multaPorDanio = 500.0m;
                    motivos.Add($"Condición '{request.Condicion}' ({multaPorDanio})");
                    montoTotal += multaPorDanio;
                }

                mensajePenalizacion = string.Join(" y ", motivos);

                var penalizacion = new Penalizacion(
                    usuarioId: prestamo.UsuarioId,
                    prestamoId: prestamo.PrestamoId,
                    diasRetraso: diasRetraso,
                    montoMora: montoTotal,
                    motivo: $"Multa por: {mensajePenalizacion} en el préstamo #{prestamo.PrestamoId}."
                );

                await _penalizaciones.AgregarAsync(penalizacion); 
            }

            await _devoluciones.AgregarAsync(nuevaDevolucion);
            await _prestamos.ActualizarAsync(prestamo);
            await _ejemplares.ActualizarAsync(prestamo.Ejemplar);

            //Auditoria y notificaciones
            string tituloLibro = prestamo.Ejemplar.RecursoBibliografico?.Titulo ?? "Recurso";

            await _auditoria.RegistrarAsync(
                UsuarioId: bibliotecarioId,
                Accion: "Registrar",
                EntidadAfectada: "Devolucion",
                detalles: $"Se registró la devolucion del préstamo #{prestamo.PrestamoId}. Condición: {request.Condicion}. Multa generada: {montoTotal}."
            );


            return new DevolucionResponse
            {
                PrestamoId = prestamo.PrestamoId,
                TituloRecurso = tituloLibro,
                FechaDevolucion = nuevaDevolucion.FechaDevolucion,
                DiasRetraso = diasRetraso,
                Condicion = nuevaDevolucion.Condicion,
                PenalizacionGenerada = generoPenalizacion,
                MontoPenalizacion = montoTotal,
                Mensaje = generoPenalizacion
                         ? $":Devolución registrada. Se generó una multa de ${montoTotal}."
                         : "Devolución registrada exitosamente sin multas."
            };
        }
    }
}
