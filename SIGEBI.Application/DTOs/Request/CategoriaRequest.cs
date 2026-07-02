
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs.Request
{
    public class CategoriaRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
