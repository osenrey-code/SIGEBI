using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;


namespace SIGEBI.Domain.Entities
{
    public class Solicitud
    {
        public int SolicitudId { get; private set; }

        public int UsuarioId { get; private set; }
        public virtual Usuario? Usuario { get; private set; }
        public int EjemplarId { get; private set; }
        public virtual Ejemplar? Ejemplar { get; private set; }

        public DateTime FechaSolicitud { get; private set; }
        public EstadoSolicitud Estado { get; private set; }
        public string? MotivoRechazo { get; private set; }

        public Solicitud(int usuarioId, int ejemplarId)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario de la solicitud es inválido.");

            if (ejemplarId <= 0)
                throw new BusinessException("El ejemplar de la solicitud es inválido.");

            UsuarioId = usuarioId;
            EjemplarId = ejemplarId;
            FechaSolicitud = DateTime.UtcNow; 
            Estado = EstadoSolicitud.Pendiente; // Toda solicitud nace estrictamente pendiente
        }

        protected Solicitud() { }

        public void Aprobar()
        {
            if (Estado != EstadoSolicitud.Pendiente)
                throw new BusinessException("Solo se pueden aprobar solicitudes en estado Pendiente.");

            Estado = EstadoSolicitud.Aprobada;
        }

        public void Rechazar(string motivo)
        {
            if (Estado != EstadoSolicitud.Pendiente)
                throw new BusinessException("Solo se pueden rechazar solicitudes en estado Pendiente.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new BusinessException("El motivo del rechazo es obligatorio.");

            Estado = EstadoSolicitud.Rechazada;
            MotivoRechazo = motivo;
        }
    }
}
