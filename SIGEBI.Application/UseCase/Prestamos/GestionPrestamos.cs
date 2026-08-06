using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Prestamos
{
    public class GestionPrestamos : IGestionPrestamos
    {
        private readonly IUsuario _usuarios;
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly ISolicitudRepository _solicitudes;
        private readonly IEjemplarRepository _ejemplares;
        private readonly IAuditoriaService _auditoria;
        private readonly IServicioNotificacion _notificaciones;
        private readonly IApplicationDbContext _db;

        public GestionPrestamos(
            IUsuario usuarios,
            IRepositorioPrestamo prestamos,
            IRepositorioPenalizacion penalizaciones,
            ISolicitudRepository solicitudes,
            IEjemplarRepository ejemplares,
            IAuditoriaService auditoria,
            IServicioNotificacion notificaciones,
            IApplicationDbContext db)
        {
            _usuarios = usuarios;
            _prestamos = prestamos;
            _penalizaciones = penalizaciones;
            _solicitudes = solicitudes;
            _ejemplares = ejemplares;
            _auditoria = auditoria;
            _notificaciones = notificaciones;
            _db = db;
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

            bool tienePrestamoActivo = await _prestamos.TienePrestamoActivoDeRecursoAsync(
                usuario.UsuarioId,
                ejemplar.RecursoBibliograficoId
            );

            if (tienePrestamoActivo)
            {
                await RegistrarAuditoriaSolicitudDenegadaAsync(
                    usuario.UsuarioId,
                    $"El usuario ya posee un préstamo activo del recurso ID {ejemplar.RecursoBibliograficoId}."
                );

                throw new BusinessException("Ya posees un préstamo activo de este libro. Debes devolverlo antes de solicitar otra copia.");
            }

            bool yaTieneSolicitud = await _solicitudes.ExisteSolicitudPendienteOActivaAsync(usuarioId, request.EjemplarId);

            if (yaTieneSolicitud)
            {
                throw new BusinessException("Ya tienes una solicitud pendiente o aprobada para este ejemplar.");
            }

            var ejemplar2 = await _ejemplares.ObtenerPorIdAsync(request.EjemplarId);

            if (ejemplar2 is null)
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
                $"Tu solicitud de préstamo del libro {nuevaSolicitud.Ejemplar?.RecursoBibliografico?.Titulo} " +
                $"fue recibida y está pendiente de revisión.",
                TipoNotificacion.SolicitudRecibida
            );

            await _auditoria.RegistrarAsync(
                UsuarioId: usuario.UsuarioId,
                Accion: "Solicitar Préstamo",
                EntidadAfectada: "Solicitudes",
                detalles: $"El usuario '{usuario.NombreCompleto}' solicitó el '{ejemplar.RecursoBibliografico?.Titulo}'."
            );

            await _db.SaveChangesAsync();

            return new SolicitudResponse
            {
                SolicitudId = nuevaSolicitud.SolicitudId,
                TituloRecurso = ejemplar.RecursoBibliografico?.Titulo ?? "Título no disponible",
                IdentificadorEjemplar = ejemplar.Identificador,
                FechaSolicitud = nuevaSolicitud.FechaSolicitud,
                Estado = nuevaSolicitud.Estado.ToString(),
                NombreUsuario = usuario.NombreCompleto,
                IdentificacionUsuario = ObtenerIdentificacionUsuario(usuario)
            };
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

            if (usuarioEjecutor is not Bibliotecario )
                throw new BusinessException("Solo un bibliotecario puede aprobar préstamos.");

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

            await _notificaciones.EnviarNotificacionAsync(
                nuevoPrestamo.UsuarioId,
                $"Tu solicitud de préstamo del libro {nuevoPrestamo.Ejemplar?.RecursoBibliografico?.Titulo}" +
                $" fue aprobado correctamente.",
                TipoNotificacion.PrestamoAprobado
            );

            await _auditoria.RegistrarAsync(
                UsuarioId: usuarioEjecutorId,
                Accion: "Aprobar Préstamo",
                EntidadAfectada: "Prestamos",
                detalles: $"Se aprobó la solicitud de '{solicitud.Usuario?.NombreCompleto}' para el prestamo del libro '{solicitud.Ejemplar.RecursoBibliografico?.Titulo}'."
            );

            await _db.SaveChangesAsync();
            return new PrestamoResponse
            {
                PrestamoId = nuevoPrestamo.PrestamoId,
                TituloRecurso = tituloLibro,
                IdentificadorEjemplar = solicitud.Ejemplar.Identificador,
                FechaInicio = nuevoPrestamo.FechaInicio,
                FechaLimite = nuevoPrestamo.FechaLimite,
                Estado = nuevoPrestamo.Estado.ToString(),
                NombreUsuario = usuarioEjecutor.NombreCompleto,
                IdentificacionUsuario = ObtenerIdentificacionUsuario(usuarioEjecutor)
            };
        }

        public async Task<IEnumerable<PrestamoResponse>> ConsultarHistorialAsync(
            ConsultarHistorialPrestamosRequest request)
        {
            Guard.NotNull(request, "Los filtros del historial de préstamos");

            if (request.RecursoBibliograficoId.HasValue &&
                request.RecursoBibliograficoId.Value <= 0)
            {
                throw new BusinessException("El recurso bibliográfico debe ser mayor que cero.");
            }

            if (request.EjemplarId.HasValue &&
                request.EjemplarId.Value <= 0)
            {
                throw new BusinessException("El ejemplar debe ser mayor que cero.");
            }

            string? identificacionFiltro = string.IsNullOrWhiteSpace(request.Identificacion)
                ? null
                : request.Identificacion.Trim();

            var historialPrestamos = await _prestamos.ConsultarHistorialAsync(
                identificacionFiltro,
                request.RecursoBibliograficoId,
                request.EjemplarId
            );

            var listaRespuesta = new List<PrestamoResponse>();

            foreach (var p in historialPrestamos)
            {
                var usuario = p.Usuario ?? await _usuarios.ObtenerporIdAsync(p.UsuarioId);

                listaRespuesta.Add(new PrestamoResponse
                {
                    PrestamoId = p.PrestamoId,
                    TituloRecurso = p.Ejemplar?.RecursoBibliografico?.Titulo ?? "Título no disponible",
                    IdentificadorEjemplar = p.Ejemplar?.Identificador ?? "N/A",
                    FechaInicio = p.FechaInicio,
                    FechaLimite = p.FechaLimite,
                    Estado = p.Estado.ToString(),
                    NombreUsuario = usuario?.NombreCompleto ?? "Desconocido",
                    IdentificacionUsuario = ObtenerIdentificacionUsuario(usuario)
                });
            }

            return listaRespuesta;
        }

        public async Task<IEnumerable<PrestamoResponse>> ConsultarPrestamosActivosAsync(
            ConsultarPrestamosActivosRequest request, int usuarioId)
        {
            Guard.NotNull(request, "Los filtros de préstamos activos");

            if (usuarioId <= 0)
                throw new BusinessException("El usuario actual es obligatorio.");

            var usuarioActual = await _usuarios.ObtenerporIdAsync(usuarioId);

            if (usuarioActual is null)
                throw new BusinessException("Usuario actual no encontrado en el sistema.");

            bool esGestor = usuarioActual is Administrador || usuarioActual is Bibliotecario;
            string? identificacionFiltro = null;

            
            if (!esGestor)
            {
                if (usuarioActual is Estudiante es)
                {
                    identificacionFiltro = es.Matricula;
                }
                else if (usuarioActual is Docente doc)
                {
                    identificacionFiltro = doc.CodigoEmpleado;
                }
            }
            else
            {
                
                if (!string.IsNullOrWhiteSpace(request.Identificacion))
                {
                    identificacionFiltro = request.Identificacion.Trim();
                }
            }

            if (request.RecursoBibliograficoId.HasValue &&
                request.RecursoBibliograficoId.Value <= 0)
            {
                throw new BusinessException("El recurso bibliográfico debe ser mayor que cero.");
            }

            if (request.EjemplarId.HasValue &&
                request.EjemplarId.Value <= 0)
            {
                throw new BusinessException("El ejemplar debe ser mayor que cero.");
            }

            
            var prestamosActivos = await _prestamos.ConsultarActivosAsync(
                identificacionFiltro,
                request.RecursoBibliograficoId,
                request.EjemplarId
            );

            var listaRespuesta = new List<PrestamoResponse>();

            foreach (var p in prestamosActivos)
            {
                var usuario = await _usuarios.ObtenerporIdAsync(p.UsuarioId);

                listaRespuesta.Add(new PrestamoResponse
                {
                    PrestamoId = p.PrestamoId,
                    TituloRecurso = p.Ejemplar?.RecursoBibliografico?.Titulo ?? "Título no disponible",
                    IdentificadorEjemplar = p.Ejemplar?.Identificador ?? "N/A",
                    FechaInicio = p.FechaInicio,
                    FechaLimite = p.FechaLimite,
                    Estado = p.Estado.ToString(),
                    NombreUsuario = usuario?.NombreCompleto ?? "Desconocido",
                    IdentificacionUsuario = ObtenerIdentificacionUsuario(usuario)
                });
            }

            return listaRespuesta;
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
                detalles: $"La solicitud de '{solicitud.Usuario?.NombreCompleto}' para el prestamo del libro '{solicitud.Ejemplar?.RecursoBibliografico?.Titulo}' " +
                $"fue rechazada automáticamente. Motivo: {motivo}"
            );

            await _notificaciones.EnviarNotificacionAsync(
               solicitud.UsuarioId,
               $"Tu solicitud de préstamo del libro {solicitud.Ejemplar?.RecursoBibliografico?.Titulo} " +
               $"fue rechazada. Motivo: {motivo}",
               TipoNotificacion.SolicitudRechazada
           );


            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<SolicitudResponse>> ConsultarTodasAsync()
        {
            var solicitudes = await _solicitudes.ObtenerTodasAsync();
            var listaRespuesta = new List<SolicitudResponse>();

            foreach (var s in solicitudes)
            {
                var usuario = await _usuarios.ObtenerporIdAsync(s.UsuarioId);

                listaRespuesta.Add(new SolicitudResponse
                {
                    SolicitudId = s.SolicitudId,
                    TituloRecurso = s.Ejemplar?.RecursoBibliografico?.Titulo ?? "Título no disponible",
                    IdentificadorEjemplar = s.Ejemplar?.Identificador ?? "N/A",
                    FechaSolicitud = s.FechaSolicitud,
                    Estado = s.Estado.ToString(),
                    MotivoRechazo = s.MotivoRechazo,
                    NombreUsuario = usuario?.NombreCompleto ?? "Desconocido",
                    IdentificacionUsuario = ObtenerIdentificacionUsuario(usuario)
                });
            }

            return listaRespuesta;
        }

        public async Task<IEnumerable<SolicitudResponse>> ConsultarSolicitudesPendientesAsync()
        {
            var pendientes = await _solicitudes.ObtenerPendientesAsync();
            var listaRespuesta = new List<SolicitudResponse>();

            foreach (var s in pendientes)
            {
                var usuario = await _usuarios.ObtenerporIdAsync(s.UsuarioId);

                listaRespuesta.Add(new SolicitudResponse
                {
                    SolicitudId = s.SolicitudId,
                    TituloRecurso = s.Ejemplar?.RecursoBibliografico?.Titulo ?? "Título no disponible",
                    IdentificadorEjemplar = s.Ejemplar?.Identificador ?? "N/A",
                    FechaSolicitud = s.FechaSolicitud,
                    Estado = s.Estado.ToString(),
                    MotivoRechazo = s.MotivoRechazo,
                    NombreUsuario = usuario?.NombreCompleto ?? "Desconocido",
                    IdentificacionUsuario = ObtenerIdentificacionUsuario(usuario)
                });
            }

            return listaRespuesta;

        }

        public async Task<SolicitudResponse?> ObtenerPorIdConDetallesAsync(int id)
        {
            if (id <= 0)
                throw new BusinessException("El identificador debe ser mayor a cero.");

            var solicitud = await _solicitudes.ObtenerConDetallesAsync(id);

            if (solicitud is null)
                return null;

            var usuario = await _usuarios.ObtenerporIdAsync(solicitud.UsuarioId);

            return new SolicitudResponse
            {
                SolicitudId = solicitud.SolicitudId,
                TituloRecurso = solicitud.Ejemplar?.RecursoBibliografico?.Titulo ?? "Título no disponible",
                IdentificadorEjemplar = solicitud.Ejemplar?.Identificador ?? "N/A",
                FechaSolicitud = solicitud.FechaSolicitud,
                Estado = solicitud.Estado.ToString(),
                MotivoRechazo = solicitud.MotivoRechazo,
                NombreUsuario = usuario?.NombreCompleto ?? "Desconocido",
                IdentificacionUsuario = ObtenerIdentificacionUsuario(usuario)
            };
        }

        public async Task<SolicitudResponse> RechazarSolicitudAsync(RechazarSolicitudRequest request, int usuarioEjecutorId)
        {
            Guard.NotNull(request, "Los datos de rechazo de la solicitud");

            if (usuarioEjecutorId <= 0)
                throw new BusinessException("El usuario ejecutor es obligatorio.");

            if (request.SolicitudId <= 0)
                throw new BusinessException("La solicitud es obligatoria.");

            if (string.IsNullOrWhiteSpace(request.MotivoRechazo))
                throw new BusinessException("El motivo de rechazo es obligatorio.");

            var usuarioEjecutor = await _usuarios.ObtenerporIdAsync(usuarioEjecutorId);

            if (usuarioEjecutor is null)
                throw new BusinessException("El usuario ejecutor no existe.");

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
                throw new BusinessException("El usuario ejecutor no está activo.");

            if (usuarioEjecutor is not Bibliotecario)
                throw new BusinessException("Solo un bibliotecario puede rechazar solicitudes de forma manual.");

            var solicitud = await _solicitudes.ObtenerConDetallesAsync(request.SolicitudId);

            if (solicitud is null)
                throw new BusinessException("La solicitud especificada no existe.");

            if (solicitud.Estado != EstadoSolicitud.Pendiente)
                throw new BusinessException("Solo se pueden rechazar solicitudes pendientes.");

            string motivo = request.MotivoRechazo.Trim();

            solicitud.Rechazar(motivo);

            await _solicitudes.ActualizarAsync(solicitud);

            await _notificaciones.EnviarNotificacionAsync(
                solicitud.UsuarioId,
                $"Tu solicitud de préstamo fue rechazada. Motivo: {motivo}",
                TipoNotificacion.SolicitudRechazada
            );

            await _auditoria.RegistrarAsync(
                UsuarioId: usuarioEjecutorId,
                Accion: "Rechazar Solicitud de Préstamo",
                EntidadAfectada: "Solicitudes",
                detalles: $"La solicitud de '{solicitud.Usuario?.NombreCompleto}' para el prestamo del libro '{solicitud.Ejemplar?.RecursoBibliografico?.Titulo}' " +
                $"fue rechazada. Motivo: {motivo}"
            );

            await _db.SaveChangesAsync();

            var usuarioSolicitante = await _usuarios.ObtenerporIdAsync(solicitud.UsuarioId);

            return new SolicitudResponse
            {
                SolicitudId = solicitud.SolicitudId,
                TituloRecurso = solicitud.Ejemplar?.RecursoBibliografico?.Titulo ?? "Título no disponible",
                IdentificadorEjemplar = solicitud.Ejemplar?.Identificador ?? "N/A",
                FechaSolicitud = solicitud.FechaSolicitud,
                Estado = solicitud.Estado.ToString(),
                MotivoRechazo = solicitud.MotivoRechazo,
                NombreUsuario = usuarioSolicitante?.NombreCompleto ?? "Desconocido",
                IdentificacionUsuario = ObtenerIdentificacionUsuario(usuarioSolicitante)
            };
        }

        private string ObtenerIdentificacionUsuario(Usuario? usuario)
        {
            if (usuario is Estudiante estudiante)
            {
                return estudiante.Matricula ?? "N/A";
            }
            else if (usuario is Docente docente)
            {
                return docente.CodigoEmpleado ?? "N/A";
            }
            return "N/A";
        }
    }
}