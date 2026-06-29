using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public class ConsultarHistorialDevolucionesRequest
    {
        public Guid UsuarioEjecutorId { get; set; }

        public Guid? UsuarioId { get; set; }
        public Guid? RecursoId { get; set; }

        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
