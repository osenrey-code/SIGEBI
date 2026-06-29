using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
{
    public class Notificacion
    {
        public Guid Id { get; private set; }

        public Guid? UsuarioDestinatarioId { get; private set; }

        public string CorreoDestinatario { get; private set; } = string.Empty;

        public string TipoEvento { get; private set; } = string.Empty;

        public string Mensaje { get; private set; } = string.Empty;

        public DateTime FechaRegistro { get; private set; }

        public string EstadoEnvio { get; private set; } = string.Empty;

        private Notificacion() { }

        public Notificacion(
            string correoDestinatario,
            string tipoEvento,
            string mensaje)
            : this(null, correoDestinatario, tipoEvento, mensaje)
        {
        }

        public Notificacion(
            Guid? usuarioDestinatarioId,
            string correoDestinatario,
            string tipoEvento,
            string mensaje)
        {
            if (string.IsNullOrWhiteSpace(correoDestinatario))
            {
                throw new BusinessException("El correo del destinatario es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(mensaje))
            {
                throw new BusinessException("El cuerpo de la notificación no puede estar vacío.");
            }

            Id = Guid.NewGuid();
            UsuarioDestinatarioId = usuarioDestinatarioId;
            CorreoDestinatario = correoDestinatario.Trim();
            TipoEvento = string.IsNullOrWhiteSpace(tipoEvento) ? "General" : tipoEvento.Trim();
            Mensaje = mensaje.Trim();
            FechaRegistro = DateTime.Now;
            EstadoEnvio = "Registrada";
        }

        public void MarcarComoEnviada()
        {
            EstadoEnvio = "Enviada";
        }

        public void MarcarComoFallida()
        {
            EstadoEnvio = "Fallida";
        }
    }
}
