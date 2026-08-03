using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.AppEscritorio.Dtos.Prestamos
{
    public class ConsultarPrestamosActivosRequest
    {
        public string? Identificacion { get; set; }
        public int? EjemplarId { get; set; }
        public int? RecursoBibliograficoId { get; set; }
    }
}
