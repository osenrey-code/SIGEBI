using System.ComponentModel.DataAnnotations;

namespace SIGEBI.AppEscritorio.Dtos.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "La identificación es obligatoria.")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; } = string.Empty;
    }
}
