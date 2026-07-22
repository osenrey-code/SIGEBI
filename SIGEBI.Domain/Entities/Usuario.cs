using SIGEBI.Domain.Common;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Domain.Entities
{
    public abstract class Usuario
    {
        public int UsuarioId { get; set; } 
        public string NombreCompleto { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;
        public EstadoUsuario Estado { get; set; }
        public string PassWord { get; set; } = string.Empty;

        public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
        public virtual ICollection<Penalizacion> Penalizciones { get; set; } = new List<Penalizacion>();
        public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

        public void Actualizar(string nombre)
        {
            Guard.NotNullOrWhiteSpace(nombre, "El nombre ");
            NombreCompleto = nombre;
        }

        public void CambiarPassword(string password)
        {
            Guard.NotNullOrWhiteSpace(password, "La contraseña ");
            PassWord = password;
        }




    }
}
