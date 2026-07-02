using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Request;

public class RegistrarRecursoRequest
{
    [Required(ErrorMessage = "El ISBN es obligatorio.")]
    public string ISBN { get; set; } = string.Empty;

    [Required(ErrorMessage = "El titulo del libro es obligatorio.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El Autor del libro es obligatorio.")]
    public string Autor { get; set; } = string.Empty;

    [Required(ErrorMessage = "La catogoria del libro es obligatoria.")]
    public int CategoriaId { get; set; } 

    [Required(ErrorMessage = "El año de publicación es obligatoria.")]
    public int AnioPublicacion { get; set; }

    [Required(ErrorMessage = "La imagen del libro es obligatoria.")]
    public string? ImagenUrl { get; set; }
}