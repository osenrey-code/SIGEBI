
namespace SIGEBI.Api.Models
{
    public class RegistrarRecursoFormRequest
    {
        public string ISBN { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public int CategoriaId { get; set; }
        public int AnioPublicado { get; set; }
        public int CantidadEjemplares { get; set; } = 1;
        public IFormFile? ImagenArchivo { get; set; }
        public string? Descripcion { get; set; }
    }
}
