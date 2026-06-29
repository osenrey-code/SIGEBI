using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;


namespace SIGEBI.Application.UseCase.Devoluciones
{
    public class ConsultarHistorialDevoluciones
    {
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IUsuario _usuarios;

        public ConsultarHistorialDevoluciones(
            IRepositorioPrestamo prestamos,
            IUsuario usuarios)
        {
            _prestamos = prestamos;
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse<List<DevolucionResponse>>> EjecutarAsync(
            ConsultarHistorialDevolucionesRequest request)
        {
            if (request.UsuarioEjecutorId == Guid.Empty)
            {
                return ResultadoOperacionResponse<List<DevolucionResponse>>.Error(
                    "El usuario ejecutor es obligatorio."
                );
            }

            if (request.FechaInicio.HasValue &&
                request.FechaFin.HasValue &&
                request.FechaInicio.Value.Date > request.FechaFin.Value.Date)
            {
                return ResultadoOperacionResponse<List<DevolucionResponse>>.Error(
                    "La fecha de inicio no puede ser mayor que la fecha final."
                );
            }

            var usuarioEjecutor = await _usuarios.ObtenerPorIdAsync(
                request.UsuarioEjecutorId
            );

            if (usuarioEjecutor is null)
            {
                return ResultadoOperacionResponse<List<DevolucionResponse>>.Error(
                    "El usuario ejecutor no existe."
                );
            }

            if (usuarioEjecutor.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<List<DevolucionResponse>>.Error(
                    "El usuario ejecutor no está activo."
                );
            }

            if (usuarioEjecutor.Tipo != TipoUsuario.Bibliotecario &&
                usuarioEjecutor.Tipo != TipoUsuario.Administrador &&
                usuarioEjecutor.Tipo != TipoUsuario.Auditor)
            {
                return ResultadoOperacionResponse<List<DevolucionResponse>>.Error(
                    "Solo un bibliotecario, administrador o auditor puede consultar el historial de devoluciones."
                );
            }

            IEnumerable<Prestamo> prestamos = await _prestamos.ObtenerTodosAsync();

            prestamos = prestamos.Where(p =>
                p.FechaDevolucion.HasValue &&
                p.Estado == EstadoPrestamo.Devuelto
            );

            if (request.UsuarioId.HasValue && request.UsuarioId.Value != Guid.Empty)
            {
                var usuario = await _usuarios.ObtenerConPerfilAsync(
                    request.UsuarioId.Value
                );

                if (usuario is null)
                {
                    return ResultadoOperacionResponse<List<DevolucionResponse>>.Error(
                        "El usuario consultado no existe."
                    );
                }

                if (usuario.PerfilLector is null)
                {
                    return ResultadoOperacionResponse<List<DevolucionResponse>>.Error(
                        "El usuario consultado no tiene perfil lector asignado."
                    );
                }

                prestamos = prestamos.Where(p =>
                    p.PerfilLectorId == usuario.PerfilLector.Id
                );
            }

            if (request.RecursoId.HasValue && request.RecursoId.Value != Guid.Empty)
            {
                prestamos = prestamos.Where(p =>
                    p.RecursoId == request.RecursoId.Value
                );
            }

            if (request.FechaInicio.HasValue)
            {
                prestamos = prestamos.Where(p =>
                    p.FechaDevolucion!.Value.Date >= request.FechaInicio.Value.Date
                );
            }

            if (request.FechaFin.HasValue)
            {
                prestamos = prestamos.Where(p =>
                    p.FechaDevolucion!.Value.Date <= request.FechaFin.Value.Date
                );
            }

            var response = prestamos
                .OrderByDescending(p => p.FechaDevolucion)
                .Select(MapearDevolucion)
                .ToList();

            return ResultadoOperacionResponse<List<DevolucionResponse>>.Ok(
                "Historial de devoluciones consultado correctamente.",
                response
            );
        }

        private static DevolucionResponse MapearDevolucion(Prestamo prestamo)
        {
            var fueTardia =
                prestamo.FechaDevolucion.HasValue &&
                prestamo.FechaLimite.HasValue &&
                prestamo.FechaDevolucion.Value.Date > prestamo.FechaLimite.Value.Date;

            var diasRetraso = fueTardia
                ? (prestamo.FechaDevolucion!.Value.Date - prestamo.FechaLimite!.Value.Date).Days
                : 0;

            return new DevolucionResponse
            {
                PrestamoId = prestamo.Id,
                PerfilLectorId = prestamo.PerfilLectorId,
                RecursoId = prestamo.RecursoId,

                FechaInicio = prestamo.FechaInicio,
                FechaLimite = prestamo.FechaLimite,
                FechaDevolucion = prestamo.FechaDevolucion!.Value,

                EstadoPrestamo = prestamo.Estado.ToString(),
                FueTardia = fueTardia,
                DiasRetraso = diasRetraso,
                PenalizacionGenerada = fueTardia
            };
        }
    }
}
