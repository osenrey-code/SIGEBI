using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Response
{
    public class ReporteInventarioResponse
    {
        public int TotalRecursos { get; set; }
        public int RecursosDisponibles { get; set; }
        public int RecursosPrestados { get; set; }
        public int RecursosReservados { get; set; }
        public int RecursosFueraDeServicio { get; set; }
    }
}
