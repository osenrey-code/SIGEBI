namespace SIGEBI.Application.DTOs.Response;

public class LogAuditoriaResponse
{
    public int AuditoriaId { get; set; }

    public int UsuarioId { get; set; }
    public string Identificacion { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;

    public string Accion { get; set; } = string.Empty;

    public string EntidadAfectada { get; set; } = string.Empty;

    public string Detalle { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; }
    
}