namespace SIGEBI.Application.DTOs.Request;

public class CambiarEstadoRecursoRequest
{
    public Guid UsuarioEjecutorId { get; set; }

    public Guid RecursoId { get; set; }

    public string NuevoEstado { get; set; } = string.Empty;
}