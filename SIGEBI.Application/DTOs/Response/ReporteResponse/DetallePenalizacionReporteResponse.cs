

namespace SIGEBI.Application.DTOs.Response.ReporteResponse
{
    public record DetallePenalizacionReporteResponse
    {
        public int PenalizacionId { get; set; }

        public int UsuarioId { get; set; }

        public string TipoUsuario { get; set; } = string.Empty;

        public string Motivo { get; set; } = string.Empty;

        public int DiasRetraso { get; set; }

        public decimal MontoMora { get; set; }

        public DateTime FechaGeneracion { get; set; }

        public string Estado { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string IdentificacionUsuario { get; set; } = string.Empty;
    }
}
