
namespace SIGEBI.AppEscritorio.Dtos.Categoria
{
    public class RegistrarCategoriaResponseDto
    {
        public string Mensaje { get; set; } = string.Empty;
        public CategoriaDto? Categoria { get; set; }
    }
}
