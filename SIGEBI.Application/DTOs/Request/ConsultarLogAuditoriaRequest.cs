namespace SIGEBI.Application.DTOs.Request;

public class ConsultarLogAuditoriaRequest
{
    public int? UsuarioId { get; set; }

    public string? Accion { get; set; }

    public string? EntidadAfectada { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }
}