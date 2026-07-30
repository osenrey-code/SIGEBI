namespace SIGEBI.AppEscritorio.Session
{
    public class UserSession
    {
        private static UserSession? _instancia;
        public static UserSession Instancia => _instancia ??= new UserSession();

        public int UsuarioId { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public string TokenJwt { get; set; } = string.Empty;

        public bool EstaAutenticado => !string.IsNullOrWhiteSpace(TokenJwt);

        public void IniciarSesion(int usuarioId, string identificacion, string nombreCompleto, string correo,
            string tipoUsuario, string token)
        {
            UsuarioId = usuarioId;
            Identificacion = identificacion;
            NombreCompleto = nombreCompleto;
            Correo = correo;
            TipoUsuario = tipoUsuario;
            TokenJwt = token;
        }

        public void CerrarSesion()
        {
            UsuarioId = 0;
            Identificacion = string.Empty;
            NombreCompleto = string.Empty;
            Correo = string.Empty;
            TipoUsuario = string.Empty;
            TokenJwt = string.Empty;
        }     
    }
}
