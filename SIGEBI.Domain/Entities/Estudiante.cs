using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Domain.Entities
{
    public class Estudiante : Usuarios
    {
        public string Matricula { get; set; } = string.Empty;
    }
}
