
namespace SIGEBI.Application.DTOs.Response
{
    public record CategoriaResponse
    {
        public int CategoriaId { get; init; }
        public string Nombre { get; init; } = string.Empty;
        public string Descripcion { get; init; } = string.Empty;
    }
}
