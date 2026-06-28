using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public class RegistrarUsuarioRequest
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string DocumentoIdentidad { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string PassWord { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
    }
}
