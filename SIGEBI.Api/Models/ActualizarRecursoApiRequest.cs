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

        // Mantiene la URL/ruta actual si no se envía un archivo nuevo
        public string? ImagenUrlActual { get; set; }

        // Archivo de imagen opcional para reemplazar la portada existente
        public IFormFile? NuevaImagenArchivo { get; set; }
    }
}
