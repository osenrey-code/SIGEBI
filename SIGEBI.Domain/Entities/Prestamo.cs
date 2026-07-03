using SIGEBI.Domain.Common;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
{
    public class Prestamo
    {
        public int PrestamoId { get; private set; }

        public int UsuarioId { get; private set; }

        public Usuario? Usuario { get; private set; }

        public DateTime FechaInicio { get; private set; }

        public DateTime? FechaLimite { get; private set; }

        public DateTime? FechaDevolucion { get; private set; }

        public string? MotivoRechazo { get; private set; }

        public EstadoPrestamo Estado { get; private set; }

        public ICollection<RecursoBibliografico> Libros { get; private set; } =
            new List<RecursoBibliografico>();

        protected Prestamo() { }

        public Prestamo(int usuarioId, DateTime fechaLimite, List<RecursoBibliografico> libros)
        {
            if (usuarioId <= 0)
                throw new BusinessException("El usuario del préstamo es inválido.");

            if (libros is null || !libros.Any())
                throw new BusinessException("El préstamo debe contener al menos un recurso.");

            FechaInicio = DateTime.Now;

            if (fechaLimite <= FechaInicio)
                throw new BusinessException("La fecha límite debe ser mayor a la fecha de inicio del préstamo.");

            UsuarioId = usuarioId;
            FechaLimite = fechaLimite;
            Estado = EstadoPrestamo.Solicitado;
            Libros = libros;
        }

        public void AprobarYEntregar(int diasPermitidos)
        {
            if (Estado != EstadoPrestamo.Solicitado)
                throw new BusinessException("Solo se pueden aprobar préstamos en estado Solicitado.");

            if (diasPermitidos <= 0)
                throw new BusinessException("Los días permitidos deben ser mayores que cero.");

            var fechaActual = DateTime.Now;

            FechaInicio = fechaActual;
            FechaLimite = fechaActual.AddDays(diasPermitidos);
            Estado = EstadoPrestamo.Activo;
        }

        public void Rechazar(string motivo)
        {
            if (Estado != EstadoPrestamo.Solicitado)
                throw new BusinessException("Solo se pueden rechazar préstamos en estado Solicitado.");

            Guard.NotNullOrWhiteSpace(motivo, "El motivo del rechazo");

            MotivoRechazo = motivo.Trim();
            Estado = EstadoPrestamo.Rechazado;
        }

        public void RegistrarDevolucion(DateTime fechaActual)
        {
            if (Estado != EstadoPrestamo.Activo && Estado != EstadoPrestamo.Vencido)
                throw new BusinessException("Solo se pueden devolver préstamos activos o vencidos.");

            FechaDevolucion = fechaActual;
            Estado = EstadoPrestamo.Devuelto;
        }

        public bool EsDevolucionTardia(DateTime fechaActual)
        {
            if (!FechaLimite.HasValue)
                throw new BusinessException("El préstamo no tiene fecha límite definida.");

            return fechaActual.Date > FechaLimite.Value.Date;
        }

        public int CalcularDiasRetraso(DateTime fechaActual)
        {
            if (!EsDevolucionTardia(fechaActual))
                return 0;

            return (fechaActual.Date - FechaLimite!.Value.Date).Days;
        }
    }
}