using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public class RegistrarDevolucionRequest
    {
        public Guid PrestamoId { get; set; }
        public Guid BibliotecarioId { get; set; }
    }
}
