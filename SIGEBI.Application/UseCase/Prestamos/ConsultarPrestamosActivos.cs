using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.UseCase.Prestamos
{
    public class ConsultarPrestamosActivos
    {
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IUsuario _usuarios;

        public ConsultarPrestamosActivos(
            IRepositorioPrestamo prestamos,
            IUsuario usuarios)
        {
            _prestamos = prestamos;
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse<List<PrestamoResponse>>> EjecutarAsync(
            ConsultarPrestamosActivosRequest request)
        {
            IEnumerable<Prestamo> prestamos;

            if (request.UsuarioId.HasValue && request.UsuarioId.Value != Guid.Empty)
            {
                var usuario = await _usuarios.ObtenerConPerfilAsync(
                    request.UsuarioId.Value
                );

                if (usuario is null)
                {
                    return ResultadoOperacionResponse<List<PrestamoResponse>>.Error(
                        "El usuario no existe."
                    );
                }

                if (usuario.PerfilLector is null)
                {
                    return ResultadoOperacionResponse<List<PrestamoResponse>>.Error(
                        "El usuario no tiene perfil lector asignado."
                    );
                }

                prestamos = await _prestamos.ObtenerActivosPorPerfilLectorAsync(
                    usuario.PerfilLector.Id
                );
            }
            else
            {
                var todosLosPrestamos = await _prestamos.ObtenerTodosAsync();

                prestamos = todosLosPrestamos.Where(p =>
                    p.Estado == EstadoPrestamo.Activo
                );
            }

            if (request.RecursoId.HasValue && request.RecursoId.Value != Guid.Empty)
            {
                prestamos = prestamos.Where(p =>
                    p.RecursoId == request.RecursoId.Value
                );
            }

            var response = prestamos
                .Select(MapearPrestamo)
                .ToList();

            return ResultadoOperacionResponse<List<PrestamoResponse>>.Ok(
                "Préstamos activos consultados correctamente.",
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
