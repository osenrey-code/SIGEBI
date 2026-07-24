using System.ComponentModel.DataAnnotations;

namespace SIGEBI.AppWeb.Models.Catalogo
{
    public class ActualizarRecursoViewModel
    {
        public int RecursoBibliograficoId { get; set; }

        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "El título del libro es obligatorio.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio.")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El año de publicación es obligatorio.")]
        public int AnioPublicado { get; set; }

        public string? ImagenUrlActual { get; set; }

        [Display(Name = "Cambiar Portada / Imagen")]
        public IFormFile? NuevaImagen { get; set; }
    }
}
