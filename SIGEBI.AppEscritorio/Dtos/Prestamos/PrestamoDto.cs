
namespace SIGEBI.AppEscritorio.Dtos.Prestamos
{
    public class PrestamoDto
    {
        public int PrestamoId { get; set; }
        public string TituloRecurso { get; set; } = string.Empty;
        public string IdentificadorEjemplar { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaLimite { get; set; }
        public string Estado { get; set; } = string.Empty;
        public bool EstaVencido { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string IdentificacionUsuario { get; set; } = string.Empty;
    }
}
