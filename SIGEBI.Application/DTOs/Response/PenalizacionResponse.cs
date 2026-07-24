namespace SIGEBI.Application.DTOs.Response;

public class PenalizacionResponse
{
    public int PenalizacionId { get; set; }

    public int UsuarioId { get; set; }
    public string IdentificacionUsuario { get; set; } = string.Empty;

    public int PrestamoId { get; set; }

    public int DiasRetraso { get; set; }

    public decimal MontoMora { get; set; }

    public string Motivo { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public DateTime FechaGeneracion { get; set; }

    public DateTime? FechaResolucion { get; set; }

    public int? UsuarioResolucionId { get; set; }

    public string? MotivoResolucion { get; set; }
}