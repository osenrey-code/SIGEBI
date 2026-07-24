using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.AppWeb.Models.Catalogo
{
    public class RegistrarRecursoViewModel
    {
        [Required(ErrorMessage = "El ISBN es obligatorio.")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "El título del libro es obligatorio.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio.")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría válida.")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El año de publicación es obligatorio.")]
        public int AnioPublicado { get; set; } = DateTime.Now.Year;

        [Range(1, 100, ErrorMessage = "La cantidad de ejemplares debe ser al menos 1.")]
        public int CantidadEjemplares { get; set; } = 1;

        [Display(Name = "Portada / Imagen del recurso")]
        public IFormFile? Imagen { get; set; }
    }
}