using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.AppWeb.Models.Catalogo
{
    public class CatalogoIndexViewModel
    {
        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public string? Categoria { get; set; }
        public bool SoloDisponibles { get; set; }

        public IEnumerable<RecursoResponse> Recursos { get; set; } = new List<RecursoResponse>();
    }
}