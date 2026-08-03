using System.ComponentModel.DataAnnotations;

namespace SIGEBI.AppWeb.Models.ViewModels.Solicitudes
{
    public class RegistrarSolicitudViewModel
    {
        [Required(ErrorMessage = "El identificador del ejemplar es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del ejemplar debe ser mayor a cero.")]
        [Display(Name = "ID del Ejemplar")]
        public int EjemplarId { get; set; }
    }
}
