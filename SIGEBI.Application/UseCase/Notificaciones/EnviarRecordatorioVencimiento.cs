using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.UseCase.Notificaciones
{
    public class EnviarRecordatorioVencimiento
    {
        private readonly IRepositorioPrestamo _prestamos;
        private readonly IUsuario _usuarios;
        private readonly IRepositorioNotificacion _notificaciones;
        private readonly IServicioCorreo _servicioCorreo;

        public EnviarRecordatorioVencimiento(
            IRepositorioPrestamo prestamos,
            IUsuario usuarios,
            IRepositorioNotificacion notificaciones,
            IServicioCorreo servicioCorreo)
        {
            _prestamos = prestamos;
            _usuarios = usuarios;
            _notificaciones = notificaciones;
            _servicioCorreo = servicioCorreo;
        }

        public async Task<ResultadoOperacionResponse<int>> EjecutarAsync()
        {
            var fechaDesde = DateTime.Now.Date;
            var fechaHasta = fechaDesde.AddDays(1);

            var prestamosProximos = await _prestamos.ObtenerPrestamosProximosAVencerAsync(
                fechaDesde,
                fechaHasta
            );

            var cantidadNotificacionesEnviadas = 0;

            foreach (var prestamo in prestamosProximos)
            {
                if (prestamo.PerfilLector is null)
                {
                    continue;
                }

                var usuario = await _usuarios.ObtenerPorIdAsync(
                    prestamo.PerfilLector.UsuarioId
                );

                if (usuario is null || string.IsNullOrWhiteSpace(usuario.Correo))
                {
                    continue;
                }

                var tituloRecurso = prestamo.Recurso?.Titulo ?? "recurso bibliográfico";

                var asunto = "Recordatorio de vencimiento de préstamo";

                var mensaje =
                    $"Hola {usuario.NombreCompleto}, te recordamos que el préstamo del recurso '{tituloRecurso}' vence el día {prestamo.FechaLimite:dd/MM/yyyy}.";

                var notificacion = new Notificacion(
                    usuario.Id,
                    usuario.Correo,
                    "RecordatorioVencimiento",
                    mensaje
                );

                try
                {
                    await _servicioCorreo.EnviarAsync(
                        usuario.Correo,
                        asunto,
                        mensaje
                    );

                    notificacion.MarcarComoEnviada();
                    cantidadNotificacionesEnviadas++;
                }
                catch
                {
                    notificacion.MarcarComoFallida();
                }

                await _notificaciones.AgregarAsync(notificacion);
            }

            return ResultadoOperacionResponse<int>.Ok(
                "Recordatorios de vencimiento procesados correctamente.",
                cantidadNotificacionesEnviadas
            );
        }
    }
}