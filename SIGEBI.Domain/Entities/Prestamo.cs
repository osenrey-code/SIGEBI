using SIGEBI.Domain.Enums;

namespace SIGEBI.Domain.Entities
{
    public class Prestamo
    {
        public Guid Id { get; private set; }
        public Guid PerfilLectorId { get; private set; }
        public Guid RecursoId { get; private set; }
        public DateTime FechaSolicitud { get; private set; }
        public DateTime? FechaEntrega { get; private set; }
        public DateTime? FechaMaximaDevolucion { get; private set; }
        public DateTime? FechaDevolucion { get; private set; }
        public EstadoPrestamo Estado { get; private set; }

        
        public PerfilLector PerfilLector { get; private set; }
        public RecursoBibliografico Recurso { get; private set; }

        private Prestamo() { }

        public Prestamo(Guid PerfilLectorId, Guid RecursoId)
        {
            Id = Guid.NewGuid();
            this.PerfilLectorId = PerfilLectorId;
            this.RecursoId = RecursoId;
            FechaSolicitud = DateTime.Now;
            Estado = EstadoPrestamo.Solicitado;
        }

        public void AprobarYEntregar(int diasPermitidos)
        {
            if (Estado != EstadoPrestamo.Solicitado)
            {
                throw new Exception("Este préstamo no está Solicitado.");
            }

            FechaEntrega = DateTime.Now;
            FechaMaximaDevolucion = DateTime.Now.AddDays(diasPermitidos);
            Estado = EstadoPrestamo.Activo;
        }

        public bool RegistrarDevolucion()
        {
            if (Estado != EstadoPrestamo.Activo && Estado != EstadoPrestamo.Vencido)
            {
                throw new Exception("El préstamo no está activo.");
            }

            FechaDevolucion = DateTime.Now;
            Estado = EstadoPrestamo.Devuelto;

            return FechaDevolucion > FechaMaximaDevolucion;
        }
    }
}