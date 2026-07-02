using SIGEBI.Domain.Enums;

namespace SIGEBI.Application.DTOs.Response;

public class RecursoResponse
{
    public string ISBN { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;
    public string? ImagenUrl { get; set;  }

    public string Autor { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public int NumeroEjemplares { get; set; }
    public int AnioPublicado { get; set; } 
}