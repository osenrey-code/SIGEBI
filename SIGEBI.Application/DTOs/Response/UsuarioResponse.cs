using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Response
{
    public class UsuarioResponse
    {
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string DocumentoIdentidad { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty; 
    }
}
