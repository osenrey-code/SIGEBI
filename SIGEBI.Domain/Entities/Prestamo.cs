using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
   
{
    public class Prestamo
    {
        public Guid Id { get; private set; }
        public Guid PerfilLectorId { get; private set; }
        public Guid RecursoId { get; private set; }
        public DateTime FechaSolicitud { get; private set; }
        public DateTime FechaInicio { get; private set; }
        public DateTime FechaLimite { get; private set; }
        public DateTime? FechaDevolucion { get; private set; }
        public EstadoPrestamo Estado { get; private set; }
        public string? MotivoRechazo { get; private set; }


        public PerfilLector? PerfilLector { get; private set; }
        public RecursoBibliografico? Recurso { get; private set; }

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
                throw new BusinessException("Este préstamo no está Solicitado.");
            }

            if (diasPermitidos <= 0)
            {
                throw new BusinessException(
                    "Los días permitidos deben ser mayores que cero."
                );
            }

            FechaInicio = DateTime.Now;
            FechaLimite = DateTime.Now.AddDays(diasPermitidos);
            Estado = EstadoPrestamo.Activo;
        }

        public void RegistrarDevolucion(DateTime fechaActual)
        {
            if (Estado != EstadoPrestamo.Activo && Estado != EstadoPrestamo.Vencido)
            {
                throw new BusinessException("El préstamo no está activo.");
            }

            FechaDevolucion = fechaActual;
            Estado = EstadoPrestamo.Devuelto;
        }

        public void Rechazar(string motivo)
        {
            if (Estado != EstadoPrestamo.Solicitado)
            {
                throw new BusinessException(
                    "Solo se pueden rechazar préstamos en estado Solicitado."
                );
            }

            if (string.IsNullOrWhiteSpace(motivo))
            {
                throw new BusinessException(
                    "El motivo del rechazo es obligatorio."
                );
            }

            MotivoRechazo = motivo.Trim();
            Estado = EstadoPrestamo.Rechazado;
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