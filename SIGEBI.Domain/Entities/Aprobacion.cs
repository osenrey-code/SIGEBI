using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Domain.Entities
{
    public class Aprobacion : Resolver
    {
        public int? PrestamoGeneradoId { get; set; }
        public Prestamo Prestamo { get; set; }
    }
}
