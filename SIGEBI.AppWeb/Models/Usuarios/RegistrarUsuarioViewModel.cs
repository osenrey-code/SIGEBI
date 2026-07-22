namespace SIGEBI.AppWeb.Models.Usuarios
{
    public class RegistrarUsuarioViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
