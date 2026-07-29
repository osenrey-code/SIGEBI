using System.ComponentModel.DataAnnotations;

namespace SIGEBI.AppWeb.Models.ViewModels.Usuarios
{
    public class CambiarPasswordViewModel
    {
        [Required(ErrorMessage = "La contraseña actual es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña Actual")]
        public string PasswordActual { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public string PasswordNueva { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe confirmar la nueva contraseña.")]
        [DataType(DataType.Password)]
        [Compare("PasswordNueva", ErrorMessage = "Las contraseñas no coinciden.")]
        [Display(Name = "Confirmar Nueva Contraseña")]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }
}
