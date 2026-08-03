using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public record ConsultarUsuariosRequest
    {
        public string? nombre { get; init; } // Para filtrar por nombre o identificación
        public string? TipoUsuario { get; init; } // Ej: "estudiante", "docente"
        public string? Estado { get; init; } // Ej: "activo", "inactivo"
        public string? Identificacion { get; init; }
    }
}
