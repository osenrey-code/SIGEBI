namespace SIGEBI.AppWeb.Models.ViewModels.Usuarios
{
    public class UsuarioIndexViewModel
    {
        public string? Nombre { get; set; }
        public string? TipoUsuario { get; set; }
        public string? Estado { get; set; }
        public string? Identificacion { get; set; }
        public List<UsuarioItemViewModel> Usuarios { get; set; } = new();
    }
}
