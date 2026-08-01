using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Request
{
    public record RegistrarDevolucionRequest
    {
        [Required(ErrorMessage = "El ID del préstamo es obligatorio.")]
        public int PrestamoId { get; init; }

        [Required(ErrorMessage = "Debe indicar la condición en la que se entrega el recurso (ej. Bueno, Dañado, Extraviado).")]
        public string Condicion { get; init; } = string.Empty;
        public string? Observacion { get; init; } 
    }
}
