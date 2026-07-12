using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public record ActivarUsuarioRequest
    {
        public int UsuarioId { get; init; }
    }
}
