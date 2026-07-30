using System.Net.Http.Json;
using System.Text.Json;

namespace SIGEBI.AppEscritorio.Services.Implementaciones
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            await ValidarRespuestaAsync(response);
        }

        public async Task<byte[]> GetByteArrayAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            await ValidarRespuestaAsync(response);
            return await response.Content.ReadAsByteArrayAsync();

        }

        public async Task<T?> GetTAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            await ValidarRespuestaAsync(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task PatchAsync<TRequest>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PatchAsJsonAsync(endpoint, data);
            await ValidarRespuestaAsync(response);
            
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            await ValidarRespuestaAsync(response);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task PostAsync<TRequest>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            await ValidarRespuestaAsync(response);

        }

        public async Task PutAsync<TRequest>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            await ValidarRespuestaAsync(response);
        }

        private static async Task ValidarRespuestaAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;

            var jsonContent = await response.Content.ReadAsStringAsync();

            try
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var error = JsonSerializer.Deserialize<ApiErrorResponse>(jsonContent, options);

                throw new Exception(error?.Mensaje ?? "Ocurrió un error al procesar la solicitud en el servidor.");

            }catch (JsonException)
            {
                throw new Exception($"Error HTTP {(int)response.StatusCode}: No se pudo interpretar la respuesta.");
            }
        }

        public class ApiErrorResponse
        {
            public int StatusCode { get; set; }
            public string Mensaje { get; set; } = string.Empty;
            public string TraceId { get; set; } = string.Empty;
        }
    }
}
