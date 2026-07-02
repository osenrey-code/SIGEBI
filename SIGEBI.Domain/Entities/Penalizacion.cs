using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;
using SIGEBI.Domain.Common;

namespace SIGEBI.Domain.Entities
{
    public class Penalizacion
    {
        public int IdPenalizacion { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        public int PrestamoId { get; set; }
        public int DiasRetraso { get; private set; }
        public decimal MontoMora { get; private set; }
        public EstadoPenalizacion Estado { get; private set; }
        public DateTime FechaGeneracion { get; private set; }
        public DateTime? FechaResolucion { get; private set; } //Nullable ya que se registra luego que sea pagada
        public string Motivo { get; set; } = string.Empty;
        public Usuarios Usuario { get; set; }

  
        public Prestamo Prestamo { get; private set; } = null!;

        private Penalizacion() { }

        public Penalizacion(string UsuarioId, int PrestamoId, decimal MontoMora, string motivo)
        {
            Guard.NotNullOrWhiteSpace(UsuarioId, "El usuario ");
            Guard.GreaterThanD(MontoMora, 0, "El monto ");
            Guard.NotNullOrWhiteSpace(motivo, "El motivo ");

            this.UsuarioId = UsuarioId;
            this.PrestamoId = PrestamoId;
            FechaGeneracion = DateTime.Now;
            Estado = EstadoPenalizacion.Activa;
        }

        public void Resolver()
        {
            if (Estado != EstadoPenalizacion.Activa)
            {
                throw new BusinessException("Solo se pueden pagar penalizaciones activas.");
            }

            Estado = EstadoPenalizacion.Pagada;
            FechaResolucion = DateTime.Now;
        }
    }
}