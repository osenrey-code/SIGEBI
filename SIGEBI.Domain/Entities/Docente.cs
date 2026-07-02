using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Domain.Entities
{
    public class Docente : Usuarios
    {
        public string DNI { get; set; } = string.Empty;
    }
}
