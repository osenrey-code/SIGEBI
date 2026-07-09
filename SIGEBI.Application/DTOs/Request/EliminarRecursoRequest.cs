using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public record EliminarRecursoRequest
    {
        public int RecursoBibliograficoId { get; init; }
        public string? Motivo { get; init; }
    }
}
