namespace SIGEBI.AppWeb.Models.DTOs.Catalogo
{
    public class ConsultarCatalogoRequest
    {
        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public string? Categoria { get; set; }
        public bool? SoloDisponibles { get; set; }
    }
}
