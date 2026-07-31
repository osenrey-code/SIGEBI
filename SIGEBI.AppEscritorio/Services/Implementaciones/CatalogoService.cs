using SIGEBI.AppEscritorio.Dtos.Catalogo.Request;
using SIGEBI.AppEscritorio.Dtos.Catalogo.Response;
using SIGEBI.AppEscritorio.Services.Interfaces;
using System.Net.Http.Headers;

namespace SIGEBI.AppEscritorio.Services.Implementaciones
{
    public class CatalogoService : ICatalogoService
    {
        private readonly IApiClient _apiClient;
        private readonly string _baseUrl = "api/catalogo";

        public CatalogoService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IEnumerable<RecursoResponse>?> ConsultarTodosAsync()
        {
            return await _apiClient.GetTAsync<IEnumerable<RecursoResponse>>($"{_baseUrl}/todos");
        }

        public async Task<IEnumerable<RecursoResponse>?> ConsultarCatalogoAsync(ConsultarCatalogoRequest request)
        {
            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(request.Titulo)) queryParams.Add($"titulo={Uri.EscapeDataString(request.Titulo)}");
            if (!string.IsNullOrWhiteSpace(request.Autor)) queryParams.Add($"autor={Uri.EscapeDataString(request.Autor)}");
            if (!string.IsNullOrWhiteSpace(request.Categoria)) queryParams.Add($"categoria={Uri.EscapeDataString(request.Categoria)}");
            if (request.SoloDisponibles.HasValue) queryParams.Add($"soloDisponibles={request.SoloDisponibles}");

            string url = queryParams.Any() ? $"{_baseUrl}/consultar?{string.Join("&", queryParams)}" : $"{_baseUrl}/consultar";

            return await _apiClient.GetTAsync<IEnumerable<RecursoResponse>>(url);
        }

        public async Task<RecursoResponse?> ConsultarDetalleAsync(int id)
        {
            return await _apiClient.GetTAsync<RecursoResponse>($"{_baseUrl}/{id}");
        }

        public async Task<IEnumerable<HistorialRecursoResponse>?> ConsultarHistorialAsync(int id)
        {
            return await _apiClient.GetTAsync<IEnumerable<HistorialRecursoResponse>>($"{_baseUrl}/{id}/historial");
        }

        public async Task RegistrarRecursoAsync(RegistrarRecursoRequest request)
        {
            using var form = new MultipartFormDataContent();

            form.Add(new StringContent(request.ISBN), "ISBN");
            form.Add(new StringContent(request.Titulo), "Titulo");
            form.Add(new StringContent(request.Autor), "Autor");
            form.Add(new StringContent(request.CategoriaId.ToString()), "CategoriaId");
            form.Add(new StringContent(request.AnioPublicado.ToString()), "AnioPublicado");
            form.Add(new StringContent(request.CantidadEjemplares.ToString()), "CantidadEjemplares");

            if (!string.IsNullOrWhiteSpace(request.RutaImagenLocal) && File.Exists(request.RutaImagenLocal))
            {
                var fileBytes = await File.ReadAllBytesAsync(request.RutaImagenLocal);
                var fileContent = new ByteArrayContent(fileBytes);

                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

                form.Add(fileContent, "ImagenArchivo", Path.GetFileName(request.RutaImagenLocal));
            }

            await _apiClient.PostFormAsync<object>($"{_baseUrl}/registrar", form);
        }

        public async Task ActualizarRecursoAsync(ActualizarRecursoRequest request)
        {
            using var form = new MultipartFormDataContent();

            form.Add(new StringContent(request.RecursoBibliograficoId.ToString()), "RecursoBibliograficoId");
            form.Add(new StringContent(request.Titulo), "Titulo");
            form.Add(new StringContent(request.Autor), "Autor");
            form.Add(new StringContent(request.CategoriaId.ToString()), "CategoriaId");
            form.Add(new StringContent(request.AnioPublicado.ToString()), "AnioPublicado");

            if (!string.IsNullOrWhiteSpace(request.ImagenUrlActual))
            {
                form.Add(new StringContent(request.ImagenUrlActual), "ImagenUrlActual");
            }

            if (!string.IsNullOrWhiteSpace(request.RutaNuevaImagenLocal) && File.Exists(request.RutaNuevaImagenLocal))
            {
                var fileBytes = await File.ReadAllBytesAsync(request.RutaNuevaImagenLocal);
                var fileContent = new ByteArrayContent(fileBytes);

                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

                form.Add(fileContent, "NuevaImagenArchivo", Path.GetFileName(request.RutaNuevaImagenLocal));
            }

            await _apiClient.PutFormAsync<object>($"{_baseUrl}/actualizar", form);
        }

        public async Task CambiarEstadoRecursoAsync(CambiarEstadoRecursoRequest request)
        {
            await _apiClient.PatchAsync($"{_baseUrl}/cambiar-estado", request);
        }

        public async Task EliminarRecursoAsync(int id, string? motivo)
        {
            string url = string.IsNullOrWhiteSpace(motivo)
                ? $"{_baseUrl}/{id}"
                : $"{_baseUrl}/{id}?motivo={Uri.EscapeDataString(motivo)}";

            await _apiClient.DeleteAsync(url);
        }
    }
}