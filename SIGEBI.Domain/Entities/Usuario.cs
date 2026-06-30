using SIGEBI.Domain.Enums;

namespace SIGEBI.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public TipoUsuario Tipo { get; set; }
        public EstadoUsuario Estado { get; set; }
        public string PasswordHash { get; private set; } = string.Empty;


        //Este perfil sera para los Estudiantes o Docentes
        public PerfilLector? PerfilLector { get; private set; }

        private Usuario() { }

        public Usuario(string Identificacion, string NombreCompleto, string Correo, TipoUsuario Tipo)
        {
            Id = Guid.NewGuid();
            this.Identificacion = Identificacion;
            this.NombreCompleto = NombreCompleto;
            this.Correo = Correo;
            this.Tipo = Tipo;
            Estado = EstadoUsuario.Activo;
        }

        public void Desactivar()
        {
            Estado = EstadoUsuario.Inactivo;
        }

        public void Activar()
        {
            Estado = EstadoUsuario.Activo;
        }

        public void AsignarPerfilLector(PerfilLector perfil)
        {
            if (Tipo != TipoUsuario.Estudiante && Tipo != TipoUsuario.Docente)
                throw new Exception("Solo los estudiantes y docentes pueden tener un perfil de lector.");

            PerfilLector = perfil;
        }

        public void EstablecerPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new Exception("El hash de la contraseña es obligatorio.");

            PasswordHash = passwordHash;
        }
    }
}
