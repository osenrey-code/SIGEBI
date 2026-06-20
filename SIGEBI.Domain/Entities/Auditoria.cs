using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Domain.Entities
{
    public class Auditoria
    {
        public Guid Id { get; private set; }
        public Guid UsuarioId { get; private set; }

        public string EntidadAfectada { get; private set; }

        public Guid EntidadId { get; private set; }

        public string Accion { get; private set; }

        public string Detalle { get; private set; }

        public DateTime FechaRegistro { get; private set; }

        private Auditoria() { } 

        public Auditoria(Guid usuarioId, string entidadAfectada, Guid entidadId, string accion, string detalle)
        {
            Id = Guid.NewGuid();
            UsuarioId = usuarioId;
            EntidadAfectada = entidadAfectada;
            EntidadId = entidadId;
            Accion = accion;
            Detalle = detalle;
            FechaRegistro = DateTime.UtcNow; 
        }
    }
}
