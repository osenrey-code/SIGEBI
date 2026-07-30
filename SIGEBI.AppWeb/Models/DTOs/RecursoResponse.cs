namespace SIGEBI.AppWeb.Models.DTOs
{
    public class RecursoResponse
    {
        public int RecursoBibliograficoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; }
        public int CopiasDisponibles { get; set; }
        public int TotalEjemplares { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}