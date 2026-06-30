namespace SIGEBI.Application.DTOs.Request;

public class RegistrarRecursoRequest
{
    public Guid UsuarioEjecutorId { get; set; }

    public string Identificador { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public string Autor { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;
    public string? ImagenUrl { get; set; }

    public int NumeroEjemplares { get; set; } = 1;
}