namespace SIGEBI.Application.DTOs.Response
{
    public class ReporteInventarioResponse
    {
        public string Categoria { get; set; } = string.Empty;

        public int TotalTitulos { get; set; }

        public int TotalEjemplares { get; set; }

        public int EjemplaresDisponibles { get; set; }

        public int EjemplaresPrestados { get; set; }

        public int EjemplaresReservados { get; set; }

        public int EjemplaresFueraDeServicio { get; set; }
    }
}