namespace SIGEBI.Application.DTOs.Response;

public class PenalizacionResponse
{
    public Guid Id { get; set; }

    public Guid PerfilLectorId { get; set; }

    public Guid? UsuarioId { get; set; }

    public string Motivo { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public DateTime FechaGeneracion { get; set; }

    public DateTime? FechaResolucion { get; set; }

    public Guid? UsuarioResolucionId { get; set; }
}