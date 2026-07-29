namespace SIGEBI.AppEscritorio.Session
{
    public class UserSession
    {
        private static UserSession? _instancia;
        public static UserSession Instancia => _instancia ??= new UserSession();

        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string TokenJwt { get; set; } = string.Empty;

        public bool EstaAutenticado => !string.IsNullOrWhiteSpace(TokenJwt);

        public void IniciarSesion(int usuarioId, string nombre, string rol, string tokenJwt)
        {
            UsuarioId = usuarioId;
            Nombre = nombre;
            Rol = rol;
            TokenJwt = tokenJwt;
        }

        public void CerrarSesion()
        {
            UsuarioId = 0;
            Nombre = string.Empty;
            Rol = string.Empty;
            TokenJwt = string.Empty;

        }
        
                
    }
}
