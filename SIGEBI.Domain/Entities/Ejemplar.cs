
using SIGEBI.Domain.Exceptions;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Domain.Entities
{
    public class Ejemplar
    {
        public int EjemplarId { get; set; }

        public string Identificador { get; set; } = string.Empty;
        public int RecursoBibliograficoId { get; set; }
        public RecursoBibliografico RecursoBibliografico { get; private set; }
        public EstadoEjemplar Estado { get; set; }
        public string? Observacion { get; private set;}

        
    }
}
