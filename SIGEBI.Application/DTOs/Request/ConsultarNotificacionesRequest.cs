namespace SIGEBI.Application.DTOs.Request;

public class ConsultarNotificacionesRequest
{
    public int UsuarioEjecutorId { get; set; }

    public int? UsuarioId { get; set; }

    public string? Tipo { get; set; }
}