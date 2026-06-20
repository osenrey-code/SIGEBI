using SIGEBI.Domain.Enums;

namespace SIGEBI.Domain.Entities
{
    public class Penalizacion
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public DateTime FechaGeneracion { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public EstadoPenalizacion Estado { get; set; }

        public void ResolverPenalizacion(string motivoResolucion)
        {
            Estado = EstadoPenalizacion.Resuelta;
            FechaResolucion = DateTime.Now;
        }

        public void ResolverPenalizacion()
        {
            Estado = EstadoPenalizacion.Resuelta;
        }
    }
}