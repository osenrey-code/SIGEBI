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
    public class SolicitarPrestamo
    {
        private readonly IUsuario _usuarios;
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly ISolicitudRepository _solicitudes;
        private readonly IAuditoriaService _auditoria;
        private readonly IEjemplarRepository _ejemplares;
        private readonly IServicioNotificacion _notificaciones;

        public SolicitarPrestamo(
            IUsuario usuarios,
            IEjemplarRepository ejemplares,
            ISolicitudRepository solicitudes,
            IRepositorioPrestamo prestamos,
            IRepositorioPenalizacion penalizaciones,
            IAuditoriaService auditoria,
            IServicioNotificacion notificaciones)
        {
            _usuarios = usuarios;
            _ejemplares = ejemplares;
            _solicitudes = solicitudes;
            _prestamos = prestamos;
            _penalizaciones = penalizaciones;
            _auditoria = auditoria;
            _notificaciones = notificaciones;
        }

        public async Task<SolicitudResponse> SolicitarPrestamoAsync(
            RegistrarSolicitudRequest request,
            int usuarioId)
        {
            Guard.NotNull(request, "Los datos de la solicitud");

            if (usuarioId <= 0)
                throw new BusinessException("El usuario solicitante es obligatorio.");

            if (request.EjemplarId <= 0)
                throw new BusinessException("El ejemplar solicitado es obligatorio.");

            var usuario = await _usuarios.ObtenerporIdAsync(usuarioId);

            if (usuario is null)
                throw new BusinessException("El usuario solicitante no existe.");

            var ejemplar = await _ejemplares.ObtenerPorIdAsync(request.EjemplarId);

            if (ejemplar is null)
            {
                await RegistrarAuditoriaSolicitudDenegadaAsync(
                    usuario.UsuarioId,
                    $"El ejemplar físico ID {request.EjemplarId} no existe."
                );

                throw new BusinessException("El ejemplar físico solicitado no existe.");
            }

            if (usuario.Estado != EstadoUsuario.Activo)
            {
                await RegistrarAuditoriaSolicitudDenegadaAsync(
                    usuario.UsuarioId,
                    "El usuario solicitante no está activo."
                );

                throw new BusinessException("El usuario solicitante no está activo.");
            }

            int limitePermitido;

            if (usuario is Estudiante estudiante)
            {
                limitePermitido = estudiante.LimitePrestamos;
            }
            else if (usuario is Docente docente)
            {
                limitePermitido = docente.LimitePrestamo;
            }
            else
            {
                await RegistrarAuditoriaSolicitudDenegadaAsync(
                    usuario.UsuarioId,
                    "Solo estudiantes y docentes pueden solicitar préstamos."
                );

                throw new BusinessException("Solo estudiantes y docentes pueden solicitar préstamos.");
            }

            bool tienePenalizacion = await _penalizaciones.TienePenalizacionActivaAsync(
                usuario.UsuarioId
            );

            if (tienePenalizacion)
            {
                await RegistrarAuditoriaSolicitudDenegadaAsync(
                    usuario.UsuarioId,
                    "El usuario tiene una penalización activa."
                );

                throw new BusinessException("El usuario tiene una penalización activa y no puede solicitar recursos.");
            }

            int prestamosActivos = await _prestamos.ContarActivosPorUsuarioAsync(
                usuario.UsuarioId
            );

            if (prestamosActivos >= limitePermitido)
            {
                string motivo =
                    $"El usuario tiene {prestamosActivos} préstamo(s) activo(s) y su límite permitido es {limitePermitido}.";

                await RegistrarAuditoriaSolicitudDenegadaAsync(
                    usuario.UsuarioId,
                    motivo
                );

                throw new BusinessException($"Solicitud rechazada. {motivo}");
            }

            if (ejemplar.Estado != EstadoEjemplar.Disponible)
            {
                string motivo =
                    $"El ejemplar seleccionado no está disponible. Estado actual: {ejemplar.Estado}.";

                await RegistrarAuditoriaSolicitudDenegadaAsync(
                    usuario.UsuarioId,
                    motivo
                );

                throw new BusinessException(motivo);
            }

            var nuevaSolicitud = new Solicitud(
                usuario.UsuarioId,
                request.EjemplarId
            );

            await _solicitudes.AgregarAsync(nuevaSolicitud);

            await _notificaciones.EnviarNotificacionAsync(
                 nuevaSolicitud.UsuarioId,
                 $"Tu solicitud de préstamo #{nuevaSolicitud.SolicitudId} fue recibida y está pendiente de revisión.",
                 TipoNotificacion.SolicitudRecibida);

            await _auditoria.RegistrarAsync(
                UsuarioId: usuario.UsuarioId,
                Accion: "Solicitar Préstamo",
                EntidadAfectada: "Solicitudes",
                detalles: $"El usuario '{usuario.NombreCompleto}' solicitó el ejemplar ID {ejemplar.EjemplarId}. Préstamos activos actuales: {prestamosActivos}. Límite permitido: {limitePermitido}."
            );

            return new SolicitudResponse
            {
                SolicitudId = nuevaSolicitud.SolicitudId,
                TituloRecurso = ejemplar.RecursoBibliografico?.Titulo ?? "Título no disponible",
                IdentificadorEjemplar = ejemplar.Identificador,
                FechaSolicitud = nuevaSolicitud.FechaSolicitud,
                Estado = nuevaSolicitud.Estado.ToString()
            };
        }

        private async Task RegistrarAuditoriaSolicitudDenegadaAsync(
            int usuarioId,
            string motivo)
        {
            await _auditoria.RegistrarAsync(
                UsuarioId: usuarioId,
                Accion: "Solicitud de Préstamo Denegada",
                EntidadAfectada: "Solicitudes",
                detalles: $"La solicitud de préstamo fue denegada. Motivo: {motivo}"
            );
        }
    }
}