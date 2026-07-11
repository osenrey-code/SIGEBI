namespace SIGEBI.Application.DTOs.Request;

public class ResolverPenalizacionRequest
{
    public int PenalizacionId { get; set; }

    public string MotivoResolucion { get; set; } = string.Empty;
}