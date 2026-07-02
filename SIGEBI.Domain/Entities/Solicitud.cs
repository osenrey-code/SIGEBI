

using SIGEBI.Domain.Enums;
using System.Data;

namespace SIGEBI.Domain.Entities
{
    public class Solicitud
    {
        public int SolicitudId { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public EstadoSolicitud Estado { get; private set; }
        public string UsuarioId { get; private set; } 
        public Usuario usuario { get; set; }
        public ICollection<RecursoBibliografico> libros = new List<RecursoBibliografico>();
    }
}
