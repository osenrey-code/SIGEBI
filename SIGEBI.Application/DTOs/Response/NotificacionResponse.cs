namespace SIGEBI.Application.DTOs.Response;

public record NotificacionResponse
{
    public int NotificacionId { get; init; }

    public int UsuarioId { get; init; }

    public string Tipo { get; init; } = string.Empty;

    public string Mensaje { get; init; } = string.Empty;

    public DateTime FechaRegistro { get; init; }

    public bool Leida { get; init; }
}