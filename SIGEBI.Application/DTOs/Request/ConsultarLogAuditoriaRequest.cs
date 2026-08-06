namespace SIGEBI.Application.DTOs.Request;

public class ConsultarLogAuditoriaRequest
{
    public string? Identificacion { get; set; } = string.Empty;

    public string? Accion { get; set; }

    public string? EntidadAfectada { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }
}