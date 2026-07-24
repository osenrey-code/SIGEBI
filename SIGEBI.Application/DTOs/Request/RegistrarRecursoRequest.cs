using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Request
{
    public class RegistrarRecursoRequest
    {
        [Required(ErrorMessage = "El ISBN es obligatorio.")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "El título del libro es obligatorio.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor del libro es obligatorio.")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría del libro es obligatoria.")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El año de publicación es obligatorio.")]
        public int AnioPublicado { get; set; }

        public string? ImagenUrl { get; set; }

        public string? ImagenArchivo { get; set; }

        public int CantidadEjemplares { get; set; } = 1;
    }
}