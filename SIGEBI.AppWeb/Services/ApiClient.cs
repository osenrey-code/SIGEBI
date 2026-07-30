using System.Net.Http.Json;

namespace SIGEBI.AppWeb.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DeleteAsync(string endpoint)
        {
            AgregarTokenAutorizacion();
            var response = await _httpClient.DeleteAsync(endpoint);
            await ValidarRespuestaAsync(response);
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            AgregarTokenAutorizacion();
            var response = await _httpClient.GetAsync(endpoint);
            await ValidarRespuestaAsync(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(errorMsg) ? $"Error HTTP {response.StatusCode}" : errorMsg);
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            AgregarTokenAutorizacion();
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            await ValidarRespuestaAsync(response);
        }

        public async Task PutAsync<TRequest>(string endpoint, TRequest data)
        {
            AgregarTokenAutorizacion();
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            await ValidarRespuestaAsync(response);
        }

        public async Task PatchAsync<TRequest>(string endpoint, TRequest data)
        {
            AgregarTokenAutorizacion();
            var response = await _httpClient.PatchAsJsonAsync(endpoint, data);
            await ValidarRespuestaAsync(response);
        }

        private void AgregarTokenAutorizacion()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context?.User?.Identity?.IsAuthenticated == true)
            {

                var token = context.User.FindFirst("Token")?.Value;

                if (string.IsNullOrEmpty(token))
                {
                    token = context.GetTokenAsync("access_token").GetAwaiter().GetResult()
                         ?? context.GetTokenAsync("acces_token").GetAwaiter().GetResult();
                }

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(errorMsg) ? $"Error HTTP {response.StatusCode}" : errorMsg);
            }

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task PostAsync<TRequest>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(errorMsg) ? $"Error HTTP {response.StatusCode}" : errorMsg);
            }

      
        public class ApiErrorResponse
        {
            public int StatusCode { get; set; }
            public string Mensaje { get; set; } = string.Empty;
            public string TraceId { get; set; } = string.Empty;
        }
    }
}