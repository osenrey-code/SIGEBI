using SIGEBI.Domain.Enums;

namespace SIGEBI.Domain.Entities
{
    public class Prestamo
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid RecursoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaLimite { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public EstadoPrestamo Estado { get; set; }

        public bool EsDevolucionTardia(DateTime fechaActual) => fechaActual > FechaLimite;
        public int CalcularDiasRetraso(DateTime fechaActual) =>
            EsDevolucionTardia(fechaActual) ? (fechaActual - FechaLimite).Days : 0;
        public void RegistrarDevolucion(DateTime fechaActual)
        {
            FechaDevolucion = fechaActual;
            Estado = EstadoPrestamo.Devuelto;
        }
    }
}