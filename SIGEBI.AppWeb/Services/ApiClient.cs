using System.Net.Http.Json;

namespace SIGEBI.AppWeb.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(errorMsg) ? $"Error HTTP {response.StatusCode}" : errorMsg);
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);

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
        }
    }
}