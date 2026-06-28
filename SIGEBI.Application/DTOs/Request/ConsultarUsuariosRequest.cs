using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public class ConsultarUsuariosRequest
    {
        public string? Nombre { get; set; }
        public string? TipoUsuario { get; set; }
        public string? Estado { get; set; }
    }
}
