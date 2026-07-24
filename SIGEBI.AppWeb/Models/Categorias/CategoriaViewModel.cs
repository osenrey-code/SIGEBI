namespace SIGEBI.AppWeb.Models.Categoria
{
    public class CategoriaViewModel
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Activa { get; set; } = true;
    }
}