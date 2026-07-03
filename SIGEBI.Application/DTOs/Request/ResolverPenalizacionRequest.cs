namespace SIGEBI.Application.DTOs.Request;

public class ResolverPenalizacionRequest
{
    public int IdPenalizacion { get; set; }

    public int UsuarioResolucionId { get; set; }

    public string MotivoResolucion { get; set; } = string.Empty;
}