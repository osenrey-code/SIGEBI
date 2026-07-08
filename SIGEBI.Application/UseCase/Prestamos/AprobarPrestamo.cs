using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Prestamos
{
    public class AprobarPrestamo
    {
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IUsuario _usuarios;
        private readonly IAuditoriaService _auditoria;
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IEjemplarRepository _ejemplares;
        private readonly ISolicitudRepository _solicitudes;
        private readonly INotificador _notificador;

        public AprobarPrestamo(
            IRepositorioPrestamo prestamos,
            IUsuario usuarios,
            IAuditoriaService auditoria,
            IEjemplarRepository ejemplares,
            IRepositorioPenalizacion penalizaciones,
            ISolicitudRepository solicitudes, INotificador notificador)
        {
            _prestamos = prestamos;
            _usuarios = usuarios;
            _auditoria = auditoria;
            _ejemplares = ejemplares;
            _penalizaciones = penalizaciones;
            _solicitudes = solicitudes;
            _notificador = notificador;
        }

        public async Task<PrestamoResponse> AprobarPrestamoAsync(
            AprobarSolicitudRequest request,
            int usuarioEjecutorId)
        {
            Guard.NotNull(request, "Los datos de aprobación");

            if (usuarioEjecutorId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            if (request.SolicitudId <= 0)
                throw new BusinessException("La solicitud es obligatoria.");

            var usuarioEjecutor = await _usuarios.ObtenerporIdAsync(usuarioEjecutorId);

            if (usuarioEjecutor is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            if (usuarioEjecutor is not Bibliotecario && usuarioEjecutor is not Administrador)
                throw new BusinessException("Solo un bibliotecario o administrador puede aprobar préstamos.");

            var solicitud = await _solicitudes.ObtenerConDetallesAsync(request.SolicitudId);

            if (solicitud is null)
                throw new BusinessException("La solicitud especificada no existe.");

            if (solicitud.Estado != EstadoSolicitud.Pendiente)
                throw new BusinessException("Solo se pueden aprobar solicitudes pendientes.");

            var usuarioSolicitante = await _usuarios.ObtenerporIdAsync(solicitud.UsuarioId);

            if (usuarioSolicitante is null)
            {
                string motivo = "El usuario asociado a esta solicitud no existe.";

                await RechazarSolicitudAutomaticamenteAsync(
                    solicitud,
                    usuarioEjecutorId,
                    motivo
                );

                throw new BusinessException($"Aprobación denegada: {motivo}");
            }

            if (usuarioSolicitante.Estado != EstadoUsuario.Activo)
            {
                string motivo = "El usuario solicitante se encuentra inactivo.";

                await RechazarSolicitudAutomaticamenteAsync(
                    solicitud,
                    usuarioEjecutorId,
                    motivo
                );

                throw new BusinessException($"Aprobación denegada: {motivo}");
            }

            bool tienePenalizaciones = await _penalizaciones.TienePenalizacionActivaAsync(
                usuarioSolicitante.UsuarioId
            );

            if (tienePenalizaciones)
            {
                string motivo = "El usuario posee una penalización activa.";

                await RechazarSolicitudAutomaticamenteAsync(
                    solicitud,
                    usuarioEjecutorId,
                    motivo
                );

                throw new BusinessException($"Aprobación denegada: {motivo}");
            }

            int limiteCantidad;
            int diasPrestamo;

            if (usuarioSolicitante is Estudiante estudiante)
            {
                limiteCantidad = estudiante.LimitePrestamos;
                diasPrestamo = 7;
            }
            else if (usuarioSolicitante is Docente docente)
            {
                limiteCantidad = docente.LimitePrestamo;
                diasPrestamo = 14;
            }
            else
            {
                string motivo = "Solo estudiantes y docentes pueden recibir préstamos.";

                await RechazarSolicitudAutomaticamenteAsync(
                    solicitud,
                    usuarioEjecutorId,
                    motivo
                );

                throw new BusinessException($"Aprobación denegada: {motivo}");
            }

            int prestamosActivos = await _prestamos.ContarActivosPorUsuarioAsync(
                usuarioSolicitante.UsuarioId
            );

            if (prestamosActivos >= limiteCantidad)
            {
                string motivo =
                    $"El usuario tiene {prestamosActivos} préstamo(s) activo(s) y su límite permitido es {limiteCantidad}.";

                await RechazarSolicitudAutomaticamenteAsync(
                    solicitud,
                    usuarioEjecutorId,
                    motivo
                );

                throw new BusinessException($"Aprobación denegada: {motivo}");
            }

            if (solicitud.Ejemplar is null)
            {
                string motivo = "El ejemplar físico asociado a la solicitud no existe.";

                await RechazarSolicitudAutomaticamenteAsync(
                    solicitud,
                    usuarioEjecutorId,
                    motivo
                );

                throw new BusinessException($"Aprobación denegada: {motivo}");
            }

            if (solicitud.Ejemplar.Estado != EstadoEjemplar.Disponible)
            {
                string motivo =
                    $"El ejemplar no está disponible. Estado actual: {solicitud.Ejemplar.Estado}.";

                await RechazarSolicitudAutomaticamenteAsync(
                    solicitud,
                    usuarioEjecutorId,
                    motivo
                );

                throw new BusinessException($"Aprobación denegada: {motivo}");
            }

            solicitud.Aprobar();
            solicitud.Ejemplar.MarcarComoPrestado();

            var nuevoPrestamo = new Prestamo(
                solicitudId: solicitud.SolicitudId,
                usuarioId: usuarioSolicitante.UsuarioId,
                ejemplarId: solicitud.EjemplarId,
                diasPermitidos: diasPrestamo
            );

            await _solicitudes.ActualizarAsync(solicitud);
            await _ejemplares.ActualizarAsync(solicitud.Ejemplar);
            await _prestamos.AgregarAsync(nuevoPrestamo);

            string tituloLibro = solicitud.Ejemplar.RecursoBibliografico?.Titulo
                ?? "Recurso solicitado";

            await _auditoria.RegistrarAsync(
                UsuarioId: usuarioEjecutorId,
                Accion: "Aprobar Préstamo",
                EntidadAfectada: "Prestamos",
                detalles: $"Se aprobó la solicitud ID {solicitud.SolicitudId}. Se registró el préstamo ID {nuevoPrestamo.PrestamoId} para el usuario ID {usuarioSolicitante.UsuarioId}. Ejemplar ID {solicitud.EjemplarId}. Fecha límite: {nuevoPrestamo.FechaLimite:dd/MM/yyyy}."
            );

            return new PrestamoResponse
            {
                PrestamoId = nuevoPrestamo.PrestamoId,
                TituloRecurso = tituloLibro,
                IdentificadorEjemplar = solicitud.Ejemplar.Identificador,
                FechaInicio = nuevoPrestamo.FechaInicio,
                FechaLimite = nuevoPrestamo.FechaLimite,
                Estado = nuevoPrestamo.Estado.ToString()
            };
        }

        private async Task RechazarSolicitudAutomaticamenteAsync(
            Solicitud solicitud,
            int usuarioEjecutorId,
            string motivo)
        {
            solicitud.Rechazar(motivo);

            await _solicitudes.ActualizarAsync(solicitud);

            await _auditoria.RegistrarAsync(
                UsuarioId: usuarioEjecutorId,
                Accion: "Rechazo Automático de Solicitud",
                EntidadAfectada: "Solicitudes",
                detalles: $"La solicitud ID {solicitud.SolicitudId} fue rechazada automáticamente. Motivo: {motivo}"
            );
        }
    }
}