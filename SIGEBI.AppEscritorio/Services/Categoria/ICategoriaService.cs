using SIGEBI.AppEscritorio.Dtos.Categoria;

namespace SIGEBI.AppEscritorio.Services.Categoria
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDto>> ConsultarCategoriasAsync();
        Task<RegistrarCategoriaResponseDto?> RegistrarCategoriaAsync(RegistrarCategoriaRequestDto request);
    }
}
