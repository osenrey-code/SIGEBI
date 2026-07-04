using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;
using SIGEBI.Domain.Common;

namespace SIGEBI.Domain.Entities
{
    public class Penalizacion
    {
        public int IdPenalizacion { get; private set; }

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

        protected Penalizacion() { }

        public Penalizacion(int usuarioId, int prestamoId, int diasRetraso, decimal montoMora, string motivo)
        {
            Guard.GreaterThanD(montoMora, 0, "debe ser mayor a ");

            Guard.NotNullOrWhiteSpace(motivo, "El motivo ");
                
            UsuarioId = usuarioId;
            PrestamoId = prestamoId;
            DiasRetraso = diasRetraso;
            MontoMora = montoMora;
            Motivo = motivo;

            FechaGeneracion = DateTime.UtcNow;
            Estado = EstadoPenalizacion.Activa;
        }

        public void Resolver()
        {
            if (Estado != EstadoPenalizacion.Activa)
            {
                throw new InvalidOperationException("Solo se pueden pagar o resolver penalizaciones activas.");
            }

            Estado = EstadoPenalizacion.Pagada;
            FechaResolucion = DateTime.UtcNow;
        }
    }
}