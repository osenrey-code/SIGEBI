using System.ComponentModel.DataAnnotations;

namespace SIGEBI.AppWeb.Models.Usuarios
{
    public class DesactivarUsuarioViewModel
    {
        public int UsuarioId { get; set; }

        public string Identificacion { get; set; } = string.Empty;

        public string NombreCompleto { get; set; } = string.Empty;

        public string Motivo { get; set; } = string.Empty;
    }
}
