using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Domain.Entities
{
    public class Docente : Usuario
    {
        public string CodigoEmpleado { get; set; } = string.Empty;
    }
}
