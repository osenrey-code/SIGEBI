

using SIGEBI.Domain.Enums;

namespace SIGEBI.Domain.Entities
{
    public abstract class Usuarios
    {
        public string UsuarioId { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;
        public EstadoUsuario Estado { get; set; }
        public string PassWord { get; set; } = string.Empty;

        public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
        public virtual ICollection<Penalizacion> penalizciones { get; set; } = new List<Penalizacion>();
        public virtual ICollection<Notificacion> notificaciones { get; set; } = new List<Notificacion>();



    }
}
