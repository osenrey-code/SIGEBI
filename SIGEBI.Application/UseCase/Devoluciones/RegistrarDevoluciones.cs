using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Devoluciones
{
    public class RegistrarDevoluciones
    {

        private const decimal MontoMoraPorDia = 25m;

        private readonly IRepositorioPrestamo _prestamos;
        private readonly IRepositorioRecurso _recursos;
        private readonly IRepositorioPenalizacion _penalizaciones;
        private readonly IUsuario _usuarios;

        public RegistrarDevoluciones(
            IRepositorioPrestamo prestamos,
            IRepositorioRecurso recursos,
            IRepositorioPenalizacion penalizaciones,
            IUsuario usuarios)
        {
            _prestamos = prestamos;
            _recursos = recursos;
            _penalizaciones = penalizaciones;
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse<DevolucionResponse>> EjecutarAsync(
            RegistrarDevolucionRequest request)
        {
            if (request.PrestamoId == Guid.Empty)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "El préstamo es obligatorio."
                );
            }

            if (request.BibliotecarioId == Guid.Empty)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "El bibliotecario es obligatorio."
                );
            }

            var bibliotecario = await _usuarios.ObtenerPorIdAsync(request.BibliotecarioId);

            if (bibliotecario is null)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "El bibliotecario no existe."
                );
            }

            if (bibliotecario.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "El bibliotecario no está activo."
                );
            }

            if (bibliotecario.Tipo != TipoUsuario.Bibliotecario &&
                bibliotecario.Tipo != TipoUsuario.Administrador)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "Solo un bibliotecario o administrador puede registrar devoluciones."
                );
            }

            var prestamo = await _prestamos.ObtenerporIdAsync(request.PrestamoId);

            if (prestamo is null)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "El préstamo no existe."
                );
            }

            var recurso = await _recursos.ObtenerporIdAsync(prestamo.RecursoId);

            if (recurso is null)
            {
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    "El recurso bibliográfico asociado al préstamo no existe."
                );
            }

            var fechaDevolucion = DateTime.Now;
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
                return ResultadoOperacionResponse<DevolucionResponse>.Error(
                    ex.Message
                );
            }

            var penalizacionGenerada = false;

            if (fueTardia && diasRetraso > 0)
            {
                var penalizacion = new Penalizacion(
                    prestamo.PerfilLectorId,
                    prestamo.Id,
                    diasRetraso,
                    MontoMoraPorDia
                );

                await _penalizaciones.AgregarAsync(penalizacion);
                penalizacionGenerada = true;
            }

            await _prestamos.ActualizarAsync(prestamo);
            await _recursos.ActualizarAsync(recurso);

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

            return ResultadoOperacionResponse<DevolucionResponse>.Ok(
                "Devolución registrada correctamente.",
                response
            );
        }
    }
}
