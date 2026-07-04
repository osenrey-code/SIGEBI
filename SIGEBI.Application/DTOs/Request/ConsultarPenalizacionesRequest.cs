namespace SIGEBI.Application.DTOs.Request;

public class ConsultarPenalizacionesRequest
{
    public int UsuarioEjecutorId { get; set; }

    public int? UsuarioId { get; set; }

    public int? PrestamoId { get; set; }

    public string? Estado { get; set; }
}