using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Notificaciones
{
    public class EnviarRecordatorioVencimiento
    {
        private readonly INotificador _notificador;

        public EnviarRecordatorioVencimiento(INotificador notificador)
        {
            _notificador = notificador;
        }

        public async Task EjecutarAsync(
            int usuarioId,
            int prestamoId,
            DateTime fechaLimite)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario destinatario es obligatorio.");

            if (prestamoId <= 0)
                throw new BusinessException("El préstamo es obligatorio.");

            await _notificador.EnviarRecordatorioVencimientoAsync(
                usuarioId,
                prestamoId,
                fechaLimite
            );
        }
    }
}