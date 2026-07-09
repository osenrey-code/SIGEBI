namespace SIGEBI.Application.DTOs.Response.ReporteResponse
{
    public record ReporteInventarioResponse
    {
        public int TotalRecursos { get; set; }

        public int TotalEjemplares { get; set; }

        public int EjemplaresDisponibles { get; set; }

        public int EjemplaresPrestados { get; set; }

        public int EjemplaresReservados { get; set; }

        public int EjemplaresFueraDeServicio { get; set; }

        public List<DetalleInventarioReporteResponse> Recursos { get; set; } = [];
    }
}