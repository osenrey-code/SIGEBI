using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;
using SIGEBI.Domain.Common;

namespace SIGEBI.Domain.Entities
{
    public class Penalizacion
    {
        public int PenalizacionId { get; private set; }

        public int UsuarioId { get; private set; }
        public virtual Usuario? Usuario { get; private set; }

        public int PrestamoId { get; private set; }
        public virtual Prestamo? Prestamo { get; private set; }

        public int DiasRetraso { get; private set; }
        public decimal MontoMora { get; private set; }
        public string Motivo { get; private set; } = string.Empty;

        public EstadoPenalizacion Estado { get; private set; }
        public DateTime FechaGeneracion { get; private set; }
        public DateTime? FechaResolucion { get; private set; }

        public int? UsuarioResolucionId { get; private set; }
        public string? MotivoResolucion { get; private set; }

        protected Penalizacion() { }

        public Penalizacion(
            int usuarioId,
            int prestamoId,
            int diasRetraso,
            decimal montoMora,
            string motivo)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario de la penalización es obligatorio.");

            if (prestamoId <= 0)
                throw new BusinessException("El préstamo asociado a la penalización es obligatorio.");

            if (diasRetraso <= 0)
                throw new BusinessException("Los días de retraso deben ser mayor a cero.");

            Guard.GreaterThanD(montoMora, 0, "El monto de mora");
            Guard.NotNullOrWhiteSpace(motivo, "El motivo");

            UsuarioId = usuarioId;
            PrestamoId = prestamoId;
            DiasRetraso = diasRetraso;
            MontoMora = montoMora;
            Motivo = motivo;

            FechaGeneracion = DateTime.UtcNow;
            Estado = EstadoPenalizacion.Activa;
        }

        public void Resolver(int usuarioResolucionId, string motivoResolucion)
        {
            if (Estado != EstadoPenalizacion.Activa)
                throw new BusinessException("Solo se pueden resolver penalizaciones activas.");

            if (usuarioResolucionId <= 0)
                throw new BusinessException("El usuario que resuelve la penalización es obligatorio.");

            Guard.NotNullOrWhiteSpace(motivoResolucion, "El motivo de resolución");

            Estado = EstadoPenalizacion.Pagada;
            FechaResolucion = DateTime.UtcNow;
            UsuarioResolucionId = usuarioResolucionId;
            MotivoResolucion = motivoResolucion;
        }
    }
}