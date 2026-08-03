using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.AppEscritorio.Dtos.Reporte
{
    public class DetallePenalizacionReporteResponseDto
    {
        public int PenalizacionId { get; set; }
        public int UsuarioId { get; set; }
        public string TipoUsuario { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public int DiasRetraso { get; set; }
        public decimal MontoMora { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
