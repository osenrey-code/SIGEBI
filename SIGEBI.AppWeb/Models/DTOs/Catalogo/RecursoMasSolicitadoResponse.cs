namespace SIGEBI.AppWeb.Models.DTOs.Catalogo
{
    public class RecursoMasSolicitadoResponse
    {
        public int RecursoBibliograficoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int CantidadSolicitudes { get; set; }
        public string Autor { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; }
    }
}
