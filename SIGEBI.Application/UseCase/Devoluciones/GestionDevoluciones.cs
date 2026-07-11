using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Devoluciones
{
    public class GestionDevoluciones : IGestionDevolucionesUseCase
    {
        private const decimal MontoMoraPorDia = 25m;

        private readonly IRepositorioPrestamo _prestamos;
        private readonly IEjemplarRepository _ejemplares;
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IUsuario _usuarios;
        private readonly IRepositorioDevolucion _devoluciones;
        private readonly IAuditoriaService _auditoria;
        private readonly IServicioNotificacion _notificaciones;

        public GestionDevoluciones(
            IRepositorioPrestamo prestamos,
            IRepositorioPenalizacion penalizaciones,
            IUsuario usuarios,
            IRepositorioDevolucion devoluciones,
            IEjemplarRepository ejemplares,
            IAuditoriaService auditoria,
            IServicioNotificacion notificaciones)
        {
            _prestamos = prestamos;
            _penalizaciones = penalizaciones;
            _usuarios = usuarios;
            _devoluciones = devoluciones;
            _ejemplares = ejemplares;
            _auditoria = auditoria;
            _notificaciones = notificaciones;
        }

        public async Task<DevolucionResponse> RegistrarDevolucionAsync(
            RegistrarDevolucionRequest request,
            int bibliotecarioId)
        {
            Guard.NotNull(request, "Los datos de la devolución");

            if (bibliotecarioId <= 0)
                throw new BusinessException("El bibliotecario responsable es obligatorio.");

            if (request.PrestamoId <= 0)
                throw new BusinessException("El préstamo es obligatorio.");

            Guard.NotNullOrWhiteSpace(request.Condicion, "La condición del recurso");

            string condicion = request.Condicion.Trim();

            string? observacion = string.IsNullOrWhiteSpace(request.Observacion)
                ? null
                : request.Observacion.Trim();

            var bibliotecario = await _usuarios.ObtenerporIdAsync(
                bibliotecarioId
            );

            if (bibliotecario is null)
                throw new BusinessException("El bibliotecario responsable no existe.");

            if (bibliotecario.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El bibliotecario responsable no está activo.");

            if (bibliotecario is not Bibliotecario && bibliotecario is not Administrador)
                throw new BusinessException("Solo un bibliotecario o administrador puede registrar devoluciones.");

            var prestamo = await _prestamos.ObtenerConDetallesAsync(
                request.PrestamoId
            );

            if (prestamo is null)
                throw new BusinessException("El préstamo especificado no existe.");

            if (prestamo.Estado != EstadoPrestamo.Activo)
                throw new BusinessException("Solo se puede registrar devolución de un préstamo activo.");

            if (prestamo.Ejemplar is null)
                throw new BusinessException("El ejemplar físico asociado al préstamo no existe.");

            var devolucionExistente = await _devoluciones.ObtenerPorPrestamoIdAsync(
                prestamo.PrestamoId
            );

            if (devolucionExistente is not null)
                throw new BusinessException("Ya existe una devolución registrada para este préstamo.");

            var nuevaDevolucion = new Devolucion(
                prestamoId: prestamo.PrestamoId,
                bibliotecarioId: bibliotecarioId,
                condicion: condicion,
                observacion: observacion
            );

            int diasRetraso = prestamo.CalcularDiasRetraso(
                nuevaDevolucion.FechaDevolucion
            );

            bool tieneRetraso = diasRetraso > 0;
            bool requiereRetiroDeServicio = nuevaDevolucion.RequiereRetiro();

            decimal montoPenalizacion = 0;
            bool penalizacionGenerada = false;

            prestamo.MarcarComoDevuelto();

            prestamo.Ejemplar.RegistrarDevolucion(
                observacion
            );

            if (requiereRetiroDeServicio)
            {
                prestamo.Ejemplar.MarcarFueraDeServicio(
                    $"Retirado por condición: {condicion}. {observacion ?? "Sin observación adicional"}"
                );
            }

            if (tieneRetraso)
            {
                montoPenalizacion = diasRetraso * MontoMoraPorDia;

                var penalizacion = new Penalizacion(
                    usuarioId: prestamo.UsuarioId,
                    prestamoId: prestamo.PrestamoId,
                    diasRetraso: diasRetraso,
                    montoMora: montoPenalizacion,
                    motivo: $"Devolución tardía de {diasRetraso} día(s) en el préstamo #{prestamo.PrestamoId}."
                );

                await _penalizaciones.AgregarAsync(
                    penalizacion
                );

                await _notificaciones.EnviarNotificacionAsync(
                    penalizacion.UsuarioId,
                    $"Se ha generado una penalización por retraso en la devolución del préstamo #{prestamo.PrestamoId}.",
                    TipoNotificacion.PenalizacionGenerada
                );

                penalizacionGenerada = true;
            }

            await _devoluciones.AgregarAsync(
                nuevaDevolucion
            );

            await _prestamos.ActualizarAsync(
                prestamo
            );

            await _ejemplares.ActualizarAsync(
                prestamo.Ejemplar
            );

            string tituloRecurso = prestamo.Ejemplar.RecursoBibliografico?.Titulo
                ?? "Recurso";

            await _auditoria.RegistrarAsync(
                UsuarioId: bibliotecarioId,
                Accion: "Registrar Devolución",
                EntidadAfectada: "Devoluciones",
                detalles: $"Se registró la devolución del préstamo #{prestamo.PrestamoId}. Usuario ID {prestamo.UsuarioId}. Ejemplar ID {prestamo.EjemplarId}. Condición: {condicion}. Días de retraso: {diasRetraso}. Penalización generada: {penalizacionGenerada}. Monto: {montoPenalizacion}."
            );

            return new DevolucionResponse
            {
                PrestamoId = prestamo.PrestamoId,
                TituloRecurso = tituloRecurso,
                FechaDevolucion = nuevaDevolucion.FechaDevolucion,
                DiasRetraso = diasRetraso,
                Condicion = nuevaDevolucion.Condicion,
                PenalizacionGenerada = penalizacionGenerada,
                MontoPenalizacion = montoPenalizacion,
                Mensaje = penalizacionGenerada
                    ? $"Devolución registrada. Se generó una penalización de {montoPenalizacion} por {diasRetraso} día(s) de retraso."
                    : "Devolución registrada exitosamente sin penalización."
            };
        }

        public async Task<IEnumerable<DevolucionResponse>> ConsultarHistorialAsync(
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
                .Select(MapearDevolucion)
                .ToList();
        }

        private static DevolucionResponse MapearDevolucion(
            Devolucion devolucion)
        {
            int diasRetraso = devolucion.Prestamo is not null
                ? devolucion.Prestamo.CalcularDiasRetraso(devolucion.FechaDevolucion)
                : 0;

            decimal montoPenalizacion = diasRetraso > 0
                ? diasRetraso * MontoMoraPorDia
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
        }
    }
}