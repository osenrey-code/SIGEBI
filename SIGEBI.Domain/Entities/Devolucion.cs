
using SIGEBI.Domain.Exceptions;
using SIGEBI.Domain.Common;

namespace SIGEBI.Domain.Entities
{
    public class Devolucion
    {
        public int DevolucionId { get; private set; }
        public int PrestamoId { get; private set; }
        public virtual Prestamo? Prestamo { get; private set; }
        public int BibliotecarioId { get; private set; }
        public DateTime FechaDevolucion { get; private set; }
        public string? Observacion { get; private set; }
        public string Condicion { get; private set; } = string.Empty;

        private Devolucion() { }

        public Devolucion(int prestamoId, int bibliotecarioId, string condicion, string? observacion = null)
        {
            Guard.NotNullOrWhiteSpace(Condicion, "La condición del recurso ");
            if (prestamoId <= 0) throw new BusinessException("El ID del préstamo es inválido.");
            if (bibliotecarioId <= 0) throw new BusinessException("El ID del bibliotecario es inválido.");

            PrestamoId = prestamoId;
            BibliotecarioId = bibliotecarioId;
            Condicion = condicion;
            Observacion = observacion ?? string.Empty;
            FechaDevolucion = DateTime.UtcNow;
        }

        public bool MultaPorDanios()
        {
            return Condicion.Equals("Dañado", StringComparison.OrdinalIgnoreCase) ||
                Condicion.Equals("Extraviado", StringComparison.OrdinalIgnoreCase);
        }


    }
}
