using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public class RechazarPrestamoRequest
    {
        public Guid PrestamoId { get; set; }
        public Guid BibliotecarioId { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
