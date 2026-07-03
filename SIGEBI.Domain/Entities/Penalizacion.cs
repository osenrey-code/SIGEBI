using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
{
    public class Penalizacion
    {
        public int IdPenalizacion { get; private set; }

        public int UsuarioId { get; private set; }

        public Usuario? Usuario { get; private set; }

        public int PrestamoId { get; private set; }

        public Prestamo? Prestamo { get; private set; }

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
                throw new BusinessException("El usuario de la penalización es inválido.");

            if (prestamoId <= 0)
                throw new BusinessException("El préstamo asociado a la penalización es inválido.");

            if (diasRetraso <= 0)
                throw new BusinessException("Los días de retraso deben ser mayores que cero.");

            if (montoMora < 0)
                throw new BusinessException("El monto de mora no puede ser negativo.");

            if (string.IsNullOrWhiteSpace(motivo))
                throw new BusinessException("El motivo de la penalización es obligatorio.");

            UsuarioId = usuarioId;
            PrestamoId = prestamoId;
            DiasRetraso = diasRetraso;
            MontoMora = montoMora;
            Motivo = motivo.Trim();
            Estado = EstadoPenalizacion.Activa;
            FechaGeneracion = DateTime.Now;
        }

        public bool EstaActiva()
        {
            return Estado == EstadoPenalizacion.Activa;
        }

        public void Resolver(int usuarioResolucionId, string motivoResolucion)
        {
            if (Estado != EstadoPenalizacion.Activa)
                throw new BusinessException("Solo se pueden resolver penalizaciones activas.");

            if (usuarioResolucionId <= 0)
                throw new BusinessException("El usuario responsable de la resolución es inválido.");

            if (string.IsNullOrWhiteSpace(motivoResolucion))
                throw new BusinessException("El motivo de resolución es obligatorio.");

            Estado = EstadoPenalizacion.Resuelta;
            UsuarioResolucionId = usuarioResolucionId;
            MotivoResolucion = motivoResolucion.Trim();
            FechaResolucion = DateTime.Now;
        }

        public void Cancelar(int usuarioResolucionId, string motivoCancelacion)
        {
            if (Estado != EstadoPenalizacion.Activa)
                throw new BusinessException("Solo se pueden cancelar penalizaciones activas.");

            if (usuarioResolucionId <= 0)
                throw new BusinessException("El usuario responsable de la cancelación es inválido.");

            if (string.IsNullOrWhiteSpace(motivoCancelacion))
                throw new BusinessException("El motivo de cancelación es obligatorio.");

            Estado = EstadoPenalizacion.Cancelada;
            UsuarioResolucionId = usuarioResolucionId;
            MotivoResolucion = motivoCancelacion.Trim();
            FechaResolucion = DateTime.Now;
        }
    }
}