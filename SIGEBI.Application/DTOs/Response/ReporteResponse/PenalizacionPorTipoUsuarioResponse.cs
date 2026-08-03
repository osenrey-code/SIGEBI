
namespace SIGEBI.Application.DTOs.Response.ReporteResponse
{
    public record PenalizacionPorTipoUsuarioResponse
    {
        public string TipoUsuario { get; set; } = string.Empty;

        public int Generadas { get; set; }

        public int Activas { get; set; }

        public int Resueltas { get; set; }

        public decimal MontoTotal { get; set; }
    }
}
