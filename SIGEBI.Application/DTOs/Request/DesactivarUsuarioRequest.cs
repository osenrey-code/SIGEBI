using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public class DesactivarUsuarioRequest
    {
        public Guid UsuarioId { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
