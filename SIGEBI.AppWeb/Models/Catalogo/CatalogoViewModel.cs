using SIGEBI.AppWeb.Models.DTOs;

namespace SIGEBI.AppWeb.Models.Catalogo
{
    public class CatalogoViewModel
    {
        public string? Busqueda { get; set; }
        public IEnumerable<RecursoResponse> Recursos { get; set; } = new List<RecursoResponse>();
    }
}