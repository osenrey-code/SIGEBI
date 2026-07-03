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
        private readonly IRepositorioRecurso _recursos;
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IUsuario _usuarios;
        private readonly INotificador _notificador;
        private readonly IAuditoriaService _auditoria;

        public RegistrarDevoluciones(IRepositorioPrestamo prestamos, IRepositorioRecurso recursos,
            IRepositorioPenalizacion penalizaciones,
            IUsuario usuarios,
            INotificador notificador, IAuditoriaService auditoria)
        {
            _prestamos = prestamos;
            _recursos = recursos;
            _penalizaciones = penalizaciones;
            _usuarios = usuarios;
            _notificador = notificador;
            _auditoria = auditoria;
        }

        public async Task<ResultadoOperacionResponse<DevolucionResponse>> EjecutarAsync(
            RegistrarDevolucionRequest request)
        {
            // Validamos que venga el Id del préstamo que se está devolviendo.
            if (request.PrestamoId == Guid.Empty)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "El préstamo es obligatorio."
                );
            }

            // Validamos que venga el usuario responsable que registra la devolución.
            // Aunque se llame BibliotecarioId, también puede ser un Administrador.
            if (request.BibliotecarioId == Guid.Empty)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "El bibliotecario es obligatorio."
                );
            }

            // Buscamos al bibliotecario o administrador que registra la devolución.
            request.BibliotecarioId

            // Si no existe, no se puede registrar la devolución.
            if (bibliotecario is null)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "El bibliotecario no existe."
                );
            }

            // El responsable debe estar activo en el sistema.
            if (bibliotecario.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "El bibliotecario no está activo."
                );
            }

            // Solo Bibliotecario o Administrador pueden registrar devoluciones.
            if (bibliotecario.Tipo != TipoUsuario.Bibliotecario &&
                bibliotecario.Tipo != TipoUsuario.Administrador)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "Solo un bibliotecario o administrador puede registrar devoluciones."
                );
            }

            // Buscamos el préstamo que se quiere devolver.
            var prestamo = await _prestamos.ObtenerporIdAsync(
                request.PrestamoId
            );

            // Si el préstamo no existe, no se puede continuar.
            if (prestamo is null)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "El préstamo no existe."
                );
            }

            // Necesitamos el PerfilLector para obtener el UsuarioId del estudiante/docente
            // y poder notificarle si se genera una penalización.

            // Si no existe el perfil lector, el préstamo tiene una relación inconsistente.
            if (perfilLector is null)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "El perfil lector asociado al préstamo no existe."
                );
            }

            // Buscamos el recurso bibliográfico asociado al préstamo.
            var recurso = await _recursos.ObtenerporIdAsync(
                prestamo.RecursoId
            );

            // Si el recurso no existe, no se puede registrar correctamente la devolución.
            if (recurso is null)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "El recurso bibliográfico asociado al préstamo no existe."
                );
            }

            // Tomamos la fecha real de devolución.
            var fechaDevolucion = DateTime.Now;

            // Variables que luego usaremos para crear el response.
            bool fueTardia;
            int diasRetraso;

            try
            {
                
                fueTardia = prestamo.EsDevolucionTardia(fechaDevolucion);
                diasRetraso = prestamo.CalcularDiasRetraso(fechaDevolucion);
                prestamo.RegistrarDevolucion(fechaDevolucion);
                recurso.MarcarComoDisponible();
            }
            catch (BusinessException ex)
            {
                // Si alguna regla del dominio falla, devolvemos el mensaje.
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    ex.Message
                );
            }

            // Indica si se generó o no una penalización.
            var penalizacionGenerada = false;

            // Si la devolución fue tardía y hubo días de retraso,
            // se crea una penalización para el PerfilLector.
            if (fueTardia && diasRetraso > 0)
            {
                var penalizacion = new Penalizacion(
                    prestamo.PerfilLectorId,
                    prestamo.Id,
                    diasRetraso,
                    MontoMoraPorDia
                );

                // Guardamos la penalización en el sistema.
                await _penalizaciones.AgregarAsync(penalizacion);

                // Marcamos que sí se generó penalización para devolverlo en el response.
                penalizacionGenerada = true;

                // Notificamos al estudiante/docente asociado al PerfilLector.
                // El Notificador buscará el usuario, su correo, enviará el mensaje
                // y registrará la notificación como Enviada o Fallida.
                await _notificador.NotificarPenalizacionGeneradaAsync(
                    perfilLector.UsuarioId,
                    penalizacion.Id
                );
            }

            // Guardamos los cambios del préstamo.
            await _prestamos.ActualizarAsync(prestamo);
            await _recursos.ActualizarAsync(recurso);

            await _auditoria.RegistrarAsync(
            request.BibliotecarioId,
            "Registrar devolución",
            "Prestamo",
            prestamo.Id,
            "Exitoso",
            $"Se registró la devolución del préstamo {prestamo.Id}. " +
            $"Recurso {recurso.Id} marcado como Disponible. " +
            $"Fue tardía: {(fueTardia ? "Sí" : "No")}. " +
            $"Días de retraso: {diasRetraso}. " +
            $"Penalización generada: {(penalizacionGenerada ? "Sí" : "No")}."
            );

            // Creamos el DTO de respuesta.
            var response = new DevolucionResponse
            {
                PrestamoId = prestamo.Id,
                PerfilLectorId = prestamo.PerfilLectorId,
                RecursoId = prestamo.RecursoId,

                FechaInicio = prestamo.FechaInicio,
                FechaLimite = prestamo.FechaLimite,
                FechaDevolucion = fechaDevolucion,

                EstadoPrestamo = prestamo.Estado.ToString(),
                FueTardia = fueTardia,
                DiasRetraso = diasRetraso,
                PenalizacionGenerada = penalizacionGenerada
            };

            // Devolvemos respuesta estándar de operación exitosa.
            return ResultadoOperacionResponse<DevolucionResponse>.Ok(
                "Devolución registrada correctamente.",
                response
            );
        }
    }
}
