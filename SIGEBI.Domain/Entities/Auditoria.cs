using SIGEBI.Domain.Common;

namespace SIGEBI.Domain.Entities
{
    public class Auditoria
    {
        public int IdAuditoria { get; private set; }
        public int UsuarioId { get; private set; }

        public string EntidadAfectada { get; private set; } = string.Empty;

        public string Accion { get; private set; } = string.Empty;

        public string Detalle { get; private set; } = string.Empty;

        public DateTime FechaRegistro { get; private set; }

        private Auditoria() { } 

        public Auditoria(int usuarioId, string entidadAfectada, string accion, string detalle)
        {
            Guard.NotNullOrWhiteSpace(entidadAfectada, "La entidad que fue afectada.");
            Guard.NotNullOrWhiteSpace(accion, "La acción ");
            Guard.NotNullOrWhiteSpace(detalle, "Detallar la acción");

            UsuarioId = usuarioId;
            EntidadAfectada = entidadAfectada;
            Accion = accion;
            Detalle = detalle;
            FechaRegistro = DateTime.Now; 
        }
    }
}
