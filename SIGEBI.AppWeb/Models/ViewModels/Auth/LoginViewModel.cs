using System.ComponentModel.DataAnnotations;

namespace SIGEBI.AppWeb.Models.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [Display(Name = "Identificación")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;
    }
}
