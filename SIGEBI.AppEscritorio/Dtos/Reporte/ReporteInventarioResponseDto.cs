
namespace SIGEBI.AppEscritorio.Dtos.Reporte
{
    public class ReporteInventarioResponseDto
    {
        public int TotalRecursos { get; set; }
        public int TotalEjemplares { get; set; }
        public int EjemplaresDisponibles { get; set; }
        public int EjemplaresPrestados { get; set; }
        public int EjemplaresReservados { get; set; }
        public int EjemplaresFueraDeServicio { get; set; }
        public List<DetalleInventarioReporteResponseDto> Recursos { get; set; } = new();
    }
}

