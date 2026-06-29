namespace SIGEBI.Application.DTOs.Response;

public class HistorialRecursoResponse
{
    public Guid Id { get; set; }

    public Guid RecursoId { get; set; }

    public string TipoCambio { get; set; } = string.Empty;

    public string EstadoAnterior { get; set; } = string.Empty;

    public string EstadoNuevo { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; }

    public Guid? UsuarioResponsableId { get; set; }

    public string Responsable { get; set; } = string.Empty;

    public string Detalle { get; set; } = string.Empty;
}