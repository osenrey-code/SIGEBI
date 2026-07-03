using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
{
    public class Notificacion
    {
        public int NotificacionId { get; private set; }

        public string UsuarioId { get; private set; }

        public TipoNotificacion  Tipo { get; private set; } 

        public string Mensaje { get; private set; } = string.Empty;

        public DateTime FechaRegistro { get; private set; }
        public virtual Usuario? Usuario { get; set; }


        protected Notificacion() { }

    }
}
