namespace SIGEBI.AppEscritorio.Dtos.Auth
{
    public class AuthResult
    {
        public bool Exitoso { get; set; }
        public string MensajeError { get; set; } = string.Empty;
        public string? TipoUsuario { get; set; }

        public static AuthResult Ok(string tipoUsuario) =>
            new() { Exitoso = true, TipoUsuario = tipoUsuario };

        public static AuthResult Fallo(string mensaje) =>
            new() { Exitoso = false, MensajeError = mensaje };
    }
}
