namespace SIGEBI.Application.DTOs.Request;

public class CambiarEstadoRecursoRequest
{
    public int RecursoBibliograficoId { get; set; }

    public int EjemplarId { get; set; }

    public string NuevoEstado { get; set; } = string.Empty;

    public string? Motivo { get; set; }
}