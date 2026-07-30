using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.AppEscritorio.Dtos.Auth
{
    public class LoginResponseDto
    {
        public string Identificacion { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
