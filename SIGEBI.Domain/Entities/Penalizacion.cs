using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
{
    public class Penalizacion
    {
        public Guid Id { get; private set; }
        public Guid PerfilLectorId { get; private set; }
        public Guid PrestamoId { get; private set; }
        public int DiasRetraso { get; private set; }
        public decimal MontoMora { get; private set; }
        public EstadoPenalizacion Estado { get; private set; }
        public DateTime FechaGeneracion { get; private set; }
        public DateTime? FechaResolucion { get; private set; }
        public Guid? UsuarioResolucionId { get; private set; }
        public PerfilLector PerfilLector { get; private set; }
        public Prestamo Prestamo { get; private set; }

        private Penalizacion() { }

        public Penalizacion(Guid PerfilLectorId, Guid PrestamoId, int DiasRetraso, decimal MontoMora)
        {
            Id = Guid.NewGuid();
            this.PerfilLectorId = PerfilLectorId;
            this.PrestamoId = PrestamoId;
            this.DiasRetraso = DiasRetraso;
            this.MontoMora = DiasRetraso * MontoMora;
            Estado = EstadoPenalizacion.Activa;
        }

        public void Resolver(Guid usuarioResolucionId)
        {
            if (Estado != EstadoPenalizacion.Activa)
            {
                throw new BusinessException("Solo se pueden resolver penalizaciones activas.");
            }

            if (usuarioResolucionId == Guid.Empty)
            {
                throw new BusinessException("El usuario responsable de la resolución es obligatorio.");
            }

            Estado = EstadoPenalizacion.Resuelta;
            FechaResolucion = DateTime.Now;
            UsuarioResolucionId = usuarioResolucionId;
        }
    }
}