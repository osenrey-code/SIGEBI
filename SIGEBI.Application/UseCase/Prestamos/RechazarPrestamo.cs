using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.UseCase.Prestamos
{
    public class RechazarPrestamo
    {
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IUsuario _usuarios;

        public RechazarPrestamo(
            IRepositorioPrestamo prestamos,
            IUsuario usuarios)
        {
            _prestamos = prestamos;
            _usuarios = usuarios;
        }

        public async Task<ResultadoOperacionResponse<PrestamoResponse>> EjecutarAsync(
            RechazarPrestamoRequest request)
        {
            if (request.PrestamoId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El préstamo es obligatorio."
                );
            }

            if (request.BibliotecarioId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario es obligatorio."
                );
            }

            if (string.IsNullOrWhiteSpace(request.Motivo))
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El motivo del rechazo es obligatorio."
                );
            }

            var bibliotecario = await _usuarios.ObtenerPorIdAsync(
                request.BibliotecarioId
            );

            if (bibliotecario is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario no existe."
                );
            }

            if (bibliotecario.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario no está activo."
                );
            }

            if (bibliotecario.Tipo != TipoUsuario.Bibliotecario &&
                bibliotecario.Tipo != TipoUsuario.Administrador)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "Solo un bibliotecario o administrador puede rechazar préstamos."
                );
            }

            var prestamo = await _prestamos.ObtenerporIdAsync(
                request.PrestamoId
            );

            if (prestamo is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El préstamo no existe."
                );
            }

            try
            {
                prestamo.Rechazar(request.Motivo);
            }
            catch (BusinessException ex)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    ex.Message
                );
            }

            await _prestamos.ActualizarAsync(prestamo);

            return ResultadoOperacionResponse<PrestamoResponse>.Ok(
                "Préstamo rechazado correctamente.",
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
