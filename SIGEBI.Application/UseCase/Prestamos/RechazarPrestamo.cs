using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
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
        private readonly IAuditoriaService _auditoria;

        public RechazarPrestamo(IRepositorioPrestamo prestamos,
            IUsuario usuarios, IAuditoriaService auditoria )
        {
            _prestamos = prestamos;
            _usuarios = usuarios;
            _auditoria = auditoria;
        }

        public async Task<ResultadoOperacionResponse<PrestamoResponse>> EjecutarAsync(
            RechazarPrestamoRequest request)
        {
            // Validamos que venga el préstamo que se quiere rechazar.
            if (request.PrestamoId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El préstamo es obligatorio."
                );
            }

            // Validamos que venga el usuario responsable de rechazar el préstamo.
            // Aunque se llame BibliotecarioId, también puede ser un Administrador.
            if (request.BibliotecarioId == Guid.Empty)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario es obligatorio."
                );
            }

            // Todo rechazo debe tener un motivo.
            if (string.IsNullOrWhiteSpace(request.Motivo))
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El motivo del rechazo es obligatorio."
                );
            }

            // Buscamos al bibliotecario o administrador que ejecuta la acción.
            var bibliotecario = await _usuarios.ObtenerPorIdAsync(
                request.BibliotecarioId
            );

            if (bibliotecario is null)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario no existe."
                );
            }

            // El responsable debe estar activo.
            if (bibliotecario.Estado != EstadoUsuario.Activo)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "El bibliotecario no está activo."
                );
            }

            // Solo Bibliotecario o Administrador pueden rechazar solicitudes de préstamo.
            if (bibliotecario.Tipo != TipoUsuario.Bibliotecario &&
                bibliotecario.Tipo != TipoUsuario.Administrador)
            {
                return ResultadoOperacionResponse<PrestamoResponse>.Error(
                    "Solo un bibliotecario o administrador puede rechazar préstamos."
                );
            }

            // Buscamos el préstamo solicitado.
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

            // Guardamos el cambio del préstamo.
            await _prestamos.ActualizarAsync(prestamo);

            // Registramos auditoría después de guardar.
            // El responsable de esta acción es el bibliotecario/administrador.
            await _auditoria.RegistrarAsync(
                request.BibliotecarioId,
                "Rechazar préstamo",
                "Prestamo",
                prestamo.Id,
                "Exitoso",
                $"El préstamo fue rechazado. Motivo: {request.Motivo}"
            );

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
