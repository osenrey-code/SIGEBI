using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Response
{
    public record LoginResponse
    {
        public int UsuarioId { get; init; }
        public string NombreCompleto { get; init; } = string.Empty;
        public string Correo { get; init; } = string.Empty;
        public string TipoUsuario { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
    }
}
