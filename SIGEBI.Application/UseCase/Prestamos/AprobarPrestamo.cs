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
        private readonly INotificador _notificador;
        private readonly IAuditoriaService _auditoria;
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IEjemplarRepository _ejemplares;
        private readonly ISolicitudRepository _solicitudes;

        public AprobarPrestamo(
            IRepositorioPrestamo prestamos,
            IUsuario usuarios,
            INotificador notificador,
            IAuditoriaService auditoria,
            IEjemplarRepository ejemplares,
            IRepositorioPenalizacion penalizaciones,
            ISolicitudRepository solicitudes)
        {
            _prestamos = prestamos;
            _usuarios = usuarios;
            _notificador = notificador;
            _auditoria = auditoria;
            _ejemplares = ejemplares;
            _penalizaciones = penalizaciones;
            _solicitudes = solicitudes;
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
                throw new BusinessException("El usuario asociado a esta solicitud no existe.");

            if (usuarioSolicitante.Estado != EstadoUsuario.Activo)
                throw new BusinessException("Aprobación denegada: el usuario solicitante se encuentra inactivo.");

            bool tienePenalizaciones = await _penalizaciones.TienePenalizacionActivaAsync(
                usuarioSolicitante.UsuarioId
            );

            if (tienePenalizaciones)
            {
                solicitud.Rechazar("El usuario posee una penalización activa.");
                await _solicitudes.ActualizarAsync(solicitud);

                await _auditoria.RegistrarAsync(
                    UsuarioId: usuarioEjecutorId,
                    Accion: "Rechazar Solicitud de Préstamo",
                    EntidadAfectada: "Solicitudes",
                    detalles: $"La solicitud ID {solicitud.SolicitudId} fue rechazada automáticamente porque el usuario ID {usuarioSolicitante.UsuarioId} posee una penalización activa."
                );

                throw new BusinessException("Aprobación denegada: el usuario tiene una penalización activa. La solicitud ha sido rechazada automáticamente.");
            }

            int prestamosActivos = await _prestamos.ContarActivosPorUsuarioAsync(
                usuarioSolicitante.UsuarioId
            );

            var (limiteCantidad, diasPrestamo) = usuarioSolicitante switch
            {
                Estudiante estudiante => (estudiante.LimitePrestamos, 7),
                Docente docente => (docente.LimitePrestamo, 14),
                _ => throw new BusinessException("Solo estudiantes y docentes pueden recibir préstamos.")
            };

            if (prestamosActivos >= limiteCantidad)
            {
                throw new BusinessException(
                    $"Aprobación denegada. El usuario tiene {prestamosActivos} préstamos activos y su límite permitido es {limiteCantidad}."
                );
            }

            if (solicitud.Ejemplar is null)
                throw new BusinessException("El ejemplar físico asociado a la solicitud no existe.");

            if (solicitud.Ejemplar.Estado != EstadoEjemplar.Disponible)
                throw new BusinessException($"El ejemplar no está disponible. Estado actual: {solicitud.Ejemplar.Estado}.");

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
    }
}