namespace SIGEBI.Api.Models
{
    public class ActualizarRecursoApiRequest
    {
        public int RecursoBibliograficoId { get; set; }
        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public int CategoriaId { get; set; }
        public int AnioPublicado { get; set; }
        public int CantidadEjemplares { get; set; }
        public string? ImagenUrlActual { get; set; }
        public IFormFile? NuevaImagenArchivo { get; set; }
        public string? Descripcion { get; set; }
    }
}
