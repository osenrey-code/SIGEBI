using SIGEBI.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.DTOs.Request;


namespace SIGEBI.Application.UseCase.Prestamos
{
    public class SolicitarPrestamo
    {
       /* private readonly IRepositorioPrestamo _prestamo;
        private readonly INotificador _notificador;
        private readonly IUsuario _Usuario;
        public SolicitarPrestamo(IRepositorioPrestamo repositorio, INotificador notificador)
        {
            _repositorio = repositorio;
            _notificador = notificador;
        }

        public async Task<RegistrarPrestamoPresencialRequest> Ejecutar(PrestamoDTO dto)
        {
            var usuario = await _Usuario.ObtenerPorIdAsync(dto.UsuarioId);

            if (usuario == null)
            {
                return new RegistrarPrestamoPresencialRequest { Estado = "ERROr", Mensaje = "Usuario no encontrado" };
            }

            var prestamo = new PrestamoDTO(dto.UsuarioId, dto.RecursoId);
            await _prestamo.GuardarAsync(prestamo);

            await _notificador.EnviarAsync(usuario.Correo, "Confirmación del Préstamo", "Tu préstamo fue registrado.");
            return new RegistrarPrestamoPresencialRequest { Estado = "OK", Mensaje = "Préstamo registrado exitosamente" };
        }
       */
        
    } 
}
