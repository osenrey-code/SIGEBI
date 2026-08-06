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
            if (prestamoId <= 0) throw new BusinessException("El ID del préstamo es inválido.");
            if (bibliotecarioId <= 0) throw new BusinessException("El bibliotecario responsable de la devolución es obligatorio.");
            Guard.NotNullOrWhiteSpace(condicion, "La condición del recurso ");

            PrestamoId = prestamoId;
            BibliotecarioId = bibliotecarioId;
            Condicion = condicion.Trim();
            Observacion = observacion?.Trim();
            FechaDevolucion = DateTime.UtcNow;
        }

        public bool RequiereRetiro()
        {
            return Condicion.Equals("Deteriorado", StringComparison.OrdinalIgnoreCase) ||
                   Condicion.Equals("Inservible / Perdido", StringComparison.OrdinalIgnoreCase) ||
                   Condicion.Equals("Dañado", StringComparison.OrdinalIgnoreCase) ||
                   Condicion.Equals("Extraviado", StringComparison.OrdinalIgnoreCase);
        }
    }
}