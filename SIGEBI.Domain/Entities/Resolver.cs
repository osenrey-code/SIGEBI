using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Domain.Entities
{
    public abstract class Resolver
    {
        public int ResolverId { get; set; }
        public DateTime FechaSolucion {  get; set; }
        public string BibliotecarioId { get; set;  }
        public Usuarios Bibliotecario { get; set; }

        public int SolicitudId { get; set; }
        public Solicitud Solicitud { get; set; }
    }
}
