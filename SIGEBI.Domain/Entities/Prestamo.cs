using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
   
{
    public class Prestamo
    {
        public Guid Id { get; private set; }
        public Guid PerfilLectorId { get; private set; }
        public Guid RecursoId { get; private set; }
        public DateTime FechaInicio { get; private set; }
        public DateTime FechaLimite { get; private set; }
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
            FechaInicio = DateTime.Now;
            Estado = EstadoPrestamo.Solicitado;
        }

        public void AprobarYEntregar(int diasPermitidos)
        {
            if (Estado != EstadoPrestamo.Solicitado)
            {
                throw new BusinessExcepcion("Este préstamo no está Solicitado.");
            }

            FechaInicio = DateTime.Now;
            FechaLimite = DateTime.Now.AddDays(diasPermitidos);
            Estado = EstadoPrestamo.Activo;
        }

        public void RegistrarDevolucion(DateTime fechaActual)
        {
            if (Estado != EstadoPrestamo.Activo && Estado != EstadoPrestamo.Vencido)
            {
                throw new BusinessExcepcion("El préstamo no está activo.");
            }

            FechaDevolucion = fechaActual;
            Estado = EstadoPrestamo.Devuelto;
        }

        public bool EsDevolucionTardia(DateTime fechaActual)
        {
            return fechaActual.Date > FechaLimite.Date;
        }

        public int CalcularDiasRetraso(DateTime fechaActual)
        {
            if (!EsDevolucionTardia(fechaActual)) return 0;
            return (fechaActual.Date - FechaLimite.Date).Days;
        }
    }
}