namespace SIGEBI.AppWeb.Models.Catalogo
{
    public class CatalogoViewModel
    {
        public int RecursoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string CategoriaNombre { get; set; } = string.Empty;
        public int AnioPublicacion { get; set; }
        public int StockDisponible { get; set; }
        public bool Disponible { get; set; }
    }
}