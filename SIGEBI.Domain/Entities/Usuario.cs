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
        public int LimitePrestamos { get; set; }

        public bool VerificarHabilitacion() => Estado == EstadoUsuario.Activo;
        public bool TienePenalizacionActiva() => false; // se implementa después
        public bool PuedeSolicitarPrestamo() => VerificarHabilitacion() && !TienePenalizacionActiva();
    }
}
