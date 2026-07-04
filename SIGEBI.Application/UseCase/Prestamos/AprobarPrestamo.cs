using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Application.Interfaces.ext; 

namespace SIGEBI.Application.UseCase.Prestamos
{
    public class AprobarPrestamo
    {
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IRepositorioRecurso _recursos;
        private readonly IUsuario _usuarios;
        private readonly IRepositorioPerfilLector _perfilLector;
        private readonly INotificador _notificador;
        private readonly IAuditoriaService _auditoria;

        public AprobarPrestamo( IRepositorioPrestamo prestamos, IRepositorioRecurso recursos,
            IUsuario usuarios,
            IRepositorioPerfilLector perfilLector,
            INotificador notificador, IAuditoriaService auditoria)
        {
            _prestamos = prestamos;
            _recursos = recursos;
            _usuarios = usuarios;
            _perfilLector = perfilLector;
            _notificador = notificador;
            _auditoria = auditoria;
        }

        public async Task<ResultadoOperacionResponse<PrestamoResponse>> EjecutarAsync(
            AprobarSolicitudRequest request)
        {
            // Validamos que venga el Id del préstamo.
            if (request.PrestamoId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El préstamo es obligatorio."
                );
            }

            // Validamos que venga el Id del usuario responsable.
            if (request.EjecutorId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario es obligatorio."
                );
            }

            // Validamos que los días del préstamo sean válidos.
            if (request.DiasPermitidos <= 0)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "Los días permitidos deben ser mayores que cero."
                );
            }

            // Buscamos el usuario responsable que intenta aprobar el préstamo.
            var Ejecutor = await _usuarios.ObtenerPorIdAsync(request.EjecutorId);

            // Si no existe, no puede aprobar.
            if (Ejecutor is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario no existe."
                );
            }

            // El usuario responsable debe estar activo.
            if (Ejecutor.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario no está activo."
                );
            }

            // Solo Bibliotecario o Administrador pueden aprobar préstamos.
            if (Ejecutor.Tipo != TipoUsuario.Bibliotecario &&
                Ejecutor.Tipo != TipoUsuario.Administrador)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "Solo un bibliotecario o administrador puede aprobar préstamos."
                );
            }

            // Buscamos el préstamo solicitado.
            var prestamo = await _prestamos.ObtenerporIdAsync(request.PrestamoId);

            // Si no existe, no se puede aprobar.
            if (prestamo is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El préstamo no existe."
                );
            }

            // Solo se pueden aprobar préstamos que estén en estado Solicitado.
            // Un préstamo Activo, Devuelto o Rechazado no debe aprobarse otra vez.
            if (prestamo.Estado != EstadoPrestamo.Solicitado)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "Solo se pueden aprobar préstamos en estado Solicitado."
                );
            }

            // El préstamo guarda PerfilLectorId, no UsuarioId directamente.
            // Por eso buscamos el PerfilLector para obtener el UsuarioId del estudiante/docente.
            var perfilLector = await _perfilLector.ObtenerPorIdAsync(
                prestamo.PerfilLectorId
            );

            // Si no existe el perfil lector, el préstamo está inconsistente.
            if (perfilLector is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El perfil lector asociado al préstamo no existe."
                );
            }

            // Buscamos el recurso bibliográfico asociado al préstamo.
            var recurso = await _recursos.ObtenerporIdAsync(prestamo.RecursoId);

            // Si el recurso no existe, no se puede aprobar el préstamo.
            if (recurso is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El recurso bibliográfico asociado al préstamo no existe."
                );
            }

            // Validamos que el recurso todavía esté disponible.
            // Puede pasar que fue solicitado, pero luego otro proceso cambió su estado.
            if (!recurso.EstaDisponible())
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El recurso bibliográfico ya no está disponible."
                );
            }

            // Aquí se aplica la regla del dominio:
            // el préstamo pasa de Solicitado a Activo,
            // se asigna FechaInicio y se calcula FechaLimite.
            prestamo.AprobarYEntregar(request.DiasPermitidos);

            // El recurso cambia su estado a Prestado.
            recurso.MarcarComoPrestado();

            await _prestamos.ActualizarAsync(prestamo);
            await _recursos.ActualizarAsync(recurso);

            // Si el préstamo tiene fecha límite, notificamos al usuario lector.
            // El UsuarioId sale del PerfilLector.
            if (prestamo.FechaLimite.HasValue)
            {
                await _notificador.NotificarPrestamoAprobadoAsync(
                    perfilLector.UsuarioId,
                    prestamo.Id,
                    prestamo.FechaLimite.Value
                );
            }

            await _auditoria.RegistrarAsync(request.EjecutorId, "Aprobar préstamo", "Prestamo",
                prestamo.Id, "Exitoso", $"El prestamo fue aprobado y el recurso {recurso.Id} fue marcado como prestado.");

            // Convertimos la entidad Prestamo a un DTO de respuesta.
            var response = MapearPrestamo(prestamo);

            // Devolvemos una respuesta estándar de operación exitosa.
            return ResultadoOperacionResponse<PrestamoResponse>.Ok(
                "Préstamo aprobado correctamente.",
                response
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
