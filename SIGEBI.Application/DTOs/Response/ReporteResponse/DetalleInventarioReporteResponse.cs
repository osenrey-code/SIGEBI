using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Response.ReporteResponse
{
    public record DetalleInventarioReporteResponse
    {
        public int RecursoBibliograficoId { get; set; }

        public string ISBN { get; set; } = string.Empty;

        public string Titulo { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public int TotalEjemplares { get; set; }

        public int Disponibles { get; set; }

        public int Prestados { get; set; }

        public int Reservados { get; set; }

        public int FueraDeServicio { get; set; }
    }
}
