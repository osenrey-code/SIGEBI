using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

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
        private readonly IServicioNotificacion _notificador;

        public RegistrarDevoluciones(
            IRepositorioPrestamo prestamos,
            IRepositorioPenalizacion penalizaciones,
            IUsuario usuarios,
            IRepositorioDevolucion devoluciones,
            IEjemplarRepository ejemplares,
            IAuditoriaService auditoria,
            IServicioNotificacion notificador)
        {
            _prestamos = prestamos;
            _ejemplares = ejemplares;
            _penalizaciones = penalizaciones;
            _usuarios = usuarios;
            _devoluciones = devoluciones;
            _auditoria = auditoria;
            _notificador = notificador;
        }

        public async Task<DevolucionResponse> EjecutarAsync(
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

            var bibliotecario = await _usuarios.ObtenerporIdAsync(bibliotecarioId);

            if (bibliotecario is null)
                throw new BusinessException("El bibliotecario responsable no existe.");

            if (bibliotecario.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El bibliotecario responsable no está activo.");

            if (bibliotecario is not Bibliotecario && bibliotecario is not Administrador)
                throw new BusinessException("Solo un bibliotecario o administrador puede registrar devoluciones.");

            var prestamo = await _prestamos.ObtenerConDetallesAsync(request.PrestamoId);

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

            prestamo.Ejemplar.RegistrarDevolucion(observacion);

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

                await _penalizaciones.AgregarAsync(penalizacion);

                penalizacionGenerada = true;
            }

            await _devoluciones.AgregarAsync(nuevaDevolucion);
            await _prestamos.ActualizarAsync(prestamo);
            await _ejemplares.ActualizarAsync(prestamo.Ejemplar);

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
    }
}