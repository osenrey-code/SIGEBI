namespace SIGEBI.AppWeb.Models.ViewModels.Catalogo
{
    public class CatalogoIndexViewModel
    {
        // Filtros de búsqueda
        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public string? Categoria { get; set; }
        public bool SoloDisponibles { get; set; }

        // Lista de recursos a renderizar
        public List<RecursoItemViewModel> Recursos { get; set; } = new();
    }

    public class RecursoItemViewModel
    {
        public int RecursoBibliograficoId { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int AnioPublicado { get; set; }
        public string? ImagenUrl { get; set; }
        public int TotalEjemplares { get; set; }
        public int CopiasDisponibles { get; set; }
        public bool EstaDisponible => CopiasDisponibles > 0;
        public int? EjemplarDisponibleId { get; set; }
    }
}