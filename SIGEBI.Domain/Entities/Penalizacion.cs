using SIGEBI.Domain.Enums;

namespace SIGEBI.Domain.Entities
{
    public class Penalizacion
    {
        public Guid Id { get; private set; }
        public Guid perfilLectorId { get; private set; }
        public Guid PrestamoId { get; private set; }
        public int DiasRetraso { get; private set; }
        public decimal MontoMora { get; private set; }
        public EstadoPenalizacion Estado { get; private set; }

        public PerfilLector PerfilLector { get; private set; }
        public Prestamo Prestamo { get; private set; }

        private Penalizacion() { }

        public Penalizacion(Guid PerfilLectorId, Guid PrestamoId, int DiasRetraso, decimal MontoMora)
        {
            Id = Guid.NewGuid();
            this.perfilLectorId = PerfilLectorId;
            this.PrestamoId = PrestamoId;
            this.DiasRetraso = DiasRetraso;
            this.MontoMora = DiasRetraso * MontoMora;
            Estado = EstadoPenalizacion.Activa;
        }
    }
}