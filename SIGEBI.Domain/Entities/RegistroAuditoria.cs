using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Domain.Entities
{
    public class RegistroAuditoria
    {
        public Guid Id { get; private set; }
        public string Usuario { get; private set; }
        public string Accion { get; private set; }
        public string TablaAfectada { get; private set; }
        public DateTime FechaRegistro { get; private set; }
        public string ValoresAnteriores { get; private set; }
        public string ValoresNuevos { get; private set;  }
    }
}
