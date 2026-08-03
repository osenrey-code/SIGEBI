using SIGEBI.AppEscritorio.Dtos.Categoria;
using SIGEBI.AppEscritorio.Services.Api;
using SIGEBI.AppEscritorio.Services.Categoria;

namespace SIGEBI.AppEscritorio.Services.Categorias
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IApiClient _apiClient;

        public CategoriaService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<CategoriaDto>> ConsultarCategoriasAsync()
        {
            var resultado = await _apiClient.GetTAsync<List<CategoriaDto>>("api/categorias");
            return resultado ?? new List<CategoriaDto>();
        }

        public async Task<RegistrarCategoriaResponseDto?> RegistrarCategoriaAsync(RegistrarCategoriaRequestDto request)
        {
            return await _apiClient.PostAsync<RegistrarCategoriaRequestDto, RegistrarCategoriaResponseDto>(
                "api/categorias/registrar",
                request
            );
        }
    }
}