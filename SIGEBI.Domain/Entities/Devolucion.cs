

using SIGEBI.Domain.Common;

namespace SIGEBI.Domain.Entities
{
    public class Devolucion
    {
        public int DevolucionId { get; private set; }

        public int PrestamoId { get; private set; }

        public DateTime FechaDevolucion { get; private set; }

        public string? Observacion { get; private set; }
        public string Condicion { get; private set; } = string.Empty;

        private Devolucion() { }

        public Devolucion(int prestamoId, string Condicion, string? Observacion = null)
        {
            Guard.NotNullOrWhiteSpace(Condicion, "La condición del recurso ");
            Guard.NotNullOrWhiteSpace(Observacion, "Las observaciones del recurso ");

            PrestamoId = prestamoId;
            this.Condicion = Condicion;
            this.Observacion = Observacion ??"";
            FechaDevolucion = DateTime.Now;
        }

        public bool MultaPorDanios()
        {
            return Condicion.Equals("Dañado", StringComparison.OrdinalIgnoreCase) ||
                Condicion.Equals("Extraviado", StringComparison.OrdinalIgnoreCase);
        }


    }
}
