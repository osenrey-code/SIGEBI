using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public record ConsultarPrestamosActivosRequest
    {
        public string? Identificacion { get; init; }
        public int? EjemplarId { get; init; }
    }
}
