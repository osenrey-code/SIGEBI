namespace SIGEBI.Application.DTOs.Response;

public class RecursoResponse
{
    public Guid Id { get; set; }

    public string Identificador { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;
    public string? ImagenUrl { get; set;  }

    public string Autor { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public int NumeroEjemplares { get; set; }
}