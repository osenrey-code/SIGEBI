using SIGEBI.Domain.Common;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
   
{
    public class Prestamo
    {
        public int PrestamoId{ get; private set; }

        public DateTime FechaInicio { get; private set; }
        public DateTime FechaDevolucion { get; private set; }
        public string UsuarioId { get; private set; }
        public Usuario Usuarios { get;  set; }

        public EstadoPrestamo Estado { get; private set; }
        public ICollection<RecursoBibliografico> Libros { get; private set; } = new List<RecursoBibliografico>();

        public RecursoBibliografico? Recurso { get; private set; }

        protected Prestamo() { }

        public Prestamo(string usuarioId,DateTime FechaVencimiento, List<RecursoBibliografico> libros)
        {
            Guard.NotNullOrWhiteSpace(usuarioId, "El Usuario ");

            if (!libros.Any())
            {
                throw new BusinessException("El préstamo debe contener al menos un recurso.");
            }

            if (FechaDevolucion <= FechaInicio)
            {
                throw new BusinessException("La fecha de vencimiento debe ser mayor a la fecha de inicio del prestamo.");
            }

            UsuarioId = usuarioId;
            FechaInicio = DateTime.Now;
            Estado = EstadoPrestamo.Solicitado;
            Libros = libros;
        }

        public void AprobarYEntregar(int diasPermitidos)
        {
            if (Estado != EstadoPrestamo.Solicitado)
            {
                throw new BusinessException("Solo se pueden aprobar préstamos en estado Solicitado.");
            }

            if (diasPermitidos <= 0)
            {
                throw new BusinessException("Los días permitidos deben ser mayores que cero.");
            }

            var fechaActual = DateTime.Now;

            FechaInicio = fechaActual;
            FechaLimite = fechaActual.AddDays(diasPermitidos);
            Estado = EstadoPrestamo.Activo;
        }

        public void Rechazar(string motivo)
        {
            if (Estado != EstadoPrestamo.Solicitado)
            {
                throw new BusinessException("Solo se pueden rechazar préstamos en estado Solicitado.");
            }

            if (string.IsNullOrWhiteSpace(motivo))
            {
                throw new BusinessException("El motivo del rechazo es obligatorio.");
            }

            MotivoRechazo = motivo.Trim();
            Estado = EstadoPrestamo.Rechazado;
        }

        public void RegistrarDevolucion(DateTime fechaActual)
        {
            if (Estado != EstadoPrestamo.Activo && Estado != EstadoPrestamo.Vencido)
            {
                throw new BusinessException("Solo se pueden devolver préstamos activos o vencidos.");
            }

            FechaDevolucion = fechaActual;
            Estado = EstadoPrestamo.Devuelto;
        }

        public bool EsDevolucionTardia(DateTime fechaActual)
        {
            if (!FechaLimite.HasValue)
            {
                throw new BusinessException("El préstamo no tiene fecha límite definida.");
            }

            return fechaActual.Date > FechaLimite.Value.Date;
        }

        public int CalcularDiasRetraso(DateTime fechaActual)
        {
            if (!EsDevolucionTardia(fechaActual))
            {
                return 0;
            }

            return (fechaActual.Date - FechaLimite!.Value.Date).Days;
        }
    }
}