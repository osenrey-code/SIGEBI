using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;
using SIGEBI.Application.Interfaces.ext;


namespace SIGEBI.Application.UseCase.Prestamos
{
    public class RegistrarPrestamoPresencial
    {
        private readonly IUsuario _usuarios;
        private readonly IRepositorioRecurso _recursos;
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly INotificador _notificador;
        private readonly IAuditoriaService _auditoria;

        public RegistrarPrestamoPresencial(IUsuario usuarios, IRepositorioRecurso recursos, 
            IRepositorioPrestamo prestamos,
            IRepositorioPenalizacion penalizaciones,
            INotificador notificador, IAuditoriaService auditoria)
        {
            _usuarios = usuarios;
            _recursos = recursos;
            _prestamos = prestamos;
            _penalizaciones = penalizaciones;
            _notificador = notificador;
            _auditoria = auditoria;
        }

        public async Task<ResultadoOperacionResponse<PrestamoResponse>> EjecutarAsync(
            RegistrarPrestamoPresencialRequest request)
        {
            // Validamos que venga el usuario que recibirá el préstamo.
            // Este usuario debe ser un Estudiante o Docente.
            if (request.UsuarioId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El usuario es obligatorio."
                );
            }

            // Validamos que venga el recurso bibliográfico que se va a prestar.
            if (request.RecursoId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El recurso bibliográfico es obligatorio."
                );
            }

            // Validamos que venga el usuario responsable que registra el préstamo.
            // Aunque se llame BibliotecarioId, también puede ser un Administrador.
            if (request.EjecutorId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario es obligatorio."
                );
            }

            // Buscamos al bibliotecario o administrador que está registrando el préstamo presencial.
            var ejecutor = await _usuarios.ObtenerPorIdAsync(
                request.EjecutorId
            );

            // Si no existe, no se puede registrar el préstamo.
            if (ejecutor is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario no existe."
                );
            }

            // El bibliotecario o administrador debe estar activo para poder operar en el sistema.
            if (ejecutor.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario no está activo."
                );
            }

            // Solo un Bibliotecario o Administrador puede registrar préstamos presenciales.
            if (ejecutor.Tipo != TipoUsuario.Bibliotecario &&
                ejecutor.Tipo != TipoUsuario.Administrador)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "Solo un bibliotecario o administrador puede registrar préstamos presenciales."
                );
            }

            // Buscamos al usuario lector junto con su PerfilLector.
            var usuario = await _usuarios.ObtenerConPerfilAsync(
                request.UsuarioId
            );

            // Si el usuario no existe, no puede recibir préstamo.
            if (usuario is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El usuario no existe."
                );
            }

            // El usuario lector debe estar activo.
            if (usuario.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El usuario no está activo."
                );
            }

            // Solo Estudiantes y Docentes pueden recibir préstamos.
            if (usuario.Tipo != TipoUsuario.Estudiante &&
                usuario.Tipo != TipoUsuario.Docente)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "Solo estudiantes y docentes pueden recibir préstamos."
                );
            }

            // Validamos que el usuario tenga PerfilLector.
            if (usuario.PerfilLector is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El usuario no tiene perfil lector asignado."
                );
            }

            // Verificamos si el usuario tiene una penalización activa.
            // Si tiene penalización activa, no puede recibir nuevos préstamos.
            var penalizacionActiva = await _penalizaciones.ObtenerActivaPorPerfilLectorAsync(
                usuario.PerfilLector.Id
            );

            // Si existe una penalización activa, se rechaza el préstamo presencial.
            if (penalizacionActiva is not null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El usuario tiene una penalización activa y no puede recibir préstamos."
                );
            }

            // Buscamos el recurso bibliográfico que se quiere prestar.
            var recurso = await _recursos.ObtenerporIdAsync(
                request.RecursoId
            );

            // Si el recurso no existe, no se puede prestar.
            if (recurso is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El recurso bibliográfico no existe."
                );
            }

            // Validamos que el recurso esté disponible.
            // No se puede prestar un recurso en estado Prestado, Reservado o FueraDeServicio.
            if (!recurso.EstaDisponible())
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El recurso bibliográfico no está disponible para préstamo."
                );
            }

            // Buscamos los préstamos activos actuales del lector.
            var prestamosActivos = await _prestamos.ObtenerActivosPorPerfilLectorAsync(
                usuario.PerfilLector.Id
            );

            // Contamos cuántos préstamos activos tiene actualmente.
            var cantidadPrestamosActivos = prestamosActivos.Count();

            // Validamos si todavía puede solicitar/recibir otro préstamo.
            if (!usuario.PerfilLector.PuedeSolicitarPrestamo(cantidadPrestamosActivos))
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    $"El usuario alcanzó el límite de préstamos permitidos. Préstamos activos actuales: {cantidadPrestamosActivos}."
                );
            }

            // Creamos el préstamo asociado al PerfilLector y al Recurso.
            var prestamo = new Prestamo(
                usuario.PerfilLector.Id,
                recurso.Id
            );

            try
            {
         
                
                prestamo.AprobarYEntregar(
                    usuario.PerfilLector.DiasPrestamosPermitidos
                );

                // El recurso pasa a estado Prestado.
                recurso.MarcarComoPrestado();
            }
            catch (BusinessException ex)
            {
                // Si alguna regla del dominio falla, devolvemos el mensaje de error.
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    ex.Message
                );
            }

            // Guardamos el préstamo en el repositorio.
            await _prestamos.AgregarAsync(prestamo);
            await _recursos.ActualizarAsync(recurso);

            // Si el préstamo tiene fecha límite, notificamos al estudiante/docente.
            // Esto cumple el flujo de notificar que el préstamo fue formalizado.
            if (prestamo.FechaLimite.HasValue)
            {
                await _notificador.NotificarPrestamoAprobadoAsync(
                    usuario.Id,
                    prestamo.Id,
                    prestamo.FechaLimite.Value
                );
            }

            await _auditoria.RegistrarAsync(
                 request.EjecutorId,
                "Registrar préstamo presencial",
                "Prestamo",
                prestamo.Id,
                "Exitoso",
                $"El préstamo presencial fue registrado para el usuario {usuario.Id}. " +
                $"El recurso {recurso.Id} fue marcado como Prestado."
            );

            // Convertimos la entidad Prestamo a un DTO de respuesta.
            // Así no exponemos directamente la entidad de dominio.
            return ResultadoOperacionResponse<PrestamoResponse>.Ok(
                "Préstamo presencial registrado correctamente.",
                MapearPrestamo(prestamo)
            );
        }

        private static PrestamoResponse MapearPrestamo(Prestamo prestamo)
        {
            return new PrestamoResponse
            {
                PrestamoId = prestamo.Id,
                PerfilLectorId = prestamo.PerfilLectorId,
                RecursoId = prestamo.RecursoId,

                FechaSolicitud = prestamo.FechaSolicitud,
                FechaInicio = prestamo.FechaInicio,
                FechaLimite = prestamo.FechaLimite,
                FechaDevolucion = prestamo.FechaDevolucion,

                Estado = prestamo.Estado.ToString(),
                MotivoRechazo = prestamo.MotivoRechazo
            };
        }
    }
}
