using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public record ReporteRangoFRequest
    {
        public DateTime FechaInicio { get; init; }
        public DateTime FechaFin { get; init; }
    }
}
