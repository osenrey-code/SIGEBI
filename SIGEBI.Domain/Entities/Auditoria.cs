using SIGEBI.Domain.Common;
using SIGEBI.Domain.Exceptions;
namespace SIGEBI.Domain.Entities
{
    public class Auditoria
    {
        public int AuditoriaId { get; private set; }
        public int UsuarioId { get; private set; }

        public string EntidadAfectada { get; private set; } = string.Empty;
        public string Accion { get; private set; } = string.Empty;
        public string Detalle { get; private set; } = string.Empty;

        public DateTime FechaRegistro { get; private set; }

        private Auditoria() { } 

        public Auditoria(int usuarioId, string entidadAfectada, string accion, string detalles)
        {
            if (usuarioId <= 0) throw new BusinessException("El ID del Bibliotecario es obligatorio.");
            Guard.NotNullOrWhiteSpace(entidadAfectada, "La entidad que fue afectada.");
            Guard.NotNullOrWhiteSpace(accion, "La acción ");
            Guard.NotNullOrWhiteSpace(detalles, "El detalle de la acción");

            UsuarioId = usuarioId;
            EntidadAfectada = entidadAfectada;
            Accion = accion;
            Detalle = detalles;
            FechaRegistro = DateTime.UtcNow; 
        }
    }
}
