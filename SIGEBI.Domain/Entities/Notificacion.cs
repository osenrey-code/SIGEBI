using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
{
    public class Notificacion
    {
        public int NotificacionId { get; private set; }

        public int UsuarioId { get; private set; }

        public Usuario? Usuario { get; private set; }

        public TipoNotificacion Tipo { get; private set; }

        public string Mensaje { get; private set; } = string.Empty;

        public DateTime FechaRegistro { get; private set; }

        public bool Leida { get; private set; }

        protected Notificacion() { }

        public Notificacion(int usuarioId, TipoNotificacion tipo, string mensaje)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario destinatario de la notificación es inválido.");

            if (string.IsNullOrWhiteSpace(mensaje))
                throw new BusinessException("El mensaje de la notificación es obligatorio.");

            UsuarioId = usuarioId;
            Tipo = tipo;
            Mensaje = mensaje.Trim();
            FechaRegistro = DateTime.Now;
            Leida = false;
        }

        public void MarcarComoLeida()
        {
            Leida = true;
        }
    }
}