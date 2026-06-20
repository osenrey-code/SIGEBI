using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
{
    public class Notificacion
    {
        public Guid Id { get; private set; }
        public string CorreoDestinatario { get; private set; }
        public string TipoEvento { get; private set; }
        public string Mensaje { get; private set; }
        public DateTime FechaRegistro { get; private set; }

        private Notificacion() { }

        public Notificacion(string CorreoDestinatario, string TipoEvento, string Mensaje)
        {
            if (string.IsNullOrWhiteSpace(CorreoDestinatario))
            {
                throw new BusinessExcepcion("El correo del destinatario es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(Mensaje))
            {
                throw new BusinessExcepcion("El cuerpo de la notificación no puede estar vacío.");
            }

            Id = Guid.NewGuid();
            this.CorreoDestinatario = CorreoDestinatario;
            this.TipoEvento = string.IsNullOrWhiteSpace(TipoEvento) ? "General" : TipoEvento;
            this.Mensaje = Mensaje;
            FechaRegistro = DateTime.Now;
        }
    }
}
