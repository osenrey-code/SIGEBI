using System.ComponentModel.DataAnnotations;

namespace SIGEBI.AppWeb.Models.Devoluciones
{
    public class RegistrarDevolucionViewModel
    {
        [Required(ErrorMessage = "El ID del préstamo es obligatorio.")]
        public int PrestamoId { get; set; }

        [Required(ErrorMessage = "Debe indicar la condición en la que se entrega el recurso.")]
        public string Condicion { get; set; } = "Bueno";
        public string? Observacion { get; set; }
        public string? TituloRecurso { get; set; }
        public string? IdentificadorEjemplar { get; set; }
        public DateTime? FechaLimite { get; set; }
    }
}
