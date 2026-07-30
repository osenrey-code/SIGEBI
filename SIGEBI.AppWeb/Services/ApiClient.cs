using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace SIGEBI.AppWeb.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClient(HttpClient httpclient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpclient;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<byte[]> GetByteArrayAsync(string endpoint)
            {
            AgregarTokenAutorizacion();
            var response = await _httpClient.GetAsync(endpoint);
            await ValidarRespuestaAsync(response);
            return await response.Content.ReadAsByteArrayAsync();
            }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            AgregarTokenAutorizacion();
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            await ValidarRespuestaAsync(response);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task PostAsync<TRequest>(string endpoint, TRequest data)
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

                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
            }
        }

        private static async Task ValidarRespuestaAsync(HttpResponseMessage response)
            {
            if (response.IsSuccessStatusCode) return;

            var jsonContent = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                throw new Exception($"Error HTTP {(int)response.StatusCode}: {response.ReasonPhrase}");
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var error = JsonSerializer.Deserialize<ApiErrorResponse>(jsonContent, options);
                if (!string.IsNullOrWhiteSpace(error?.Mensaje))
                {
                    throw new Exception(error.Mensaje);
                }

                using var doc = JsonDocument.Parse(jsonContent);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Object)
                {
                    if (root.TryGetProperty("mensaje", out var msgProp) ||
                        root.TryGetProperty("message", out msgProp) ||
                        root.TryGetProperty("detail", out msgProp) ||
                        root.TryGetProperty("title", out msgProp))
                    {
                        var msg = msgProp.GetString();
                        if (!string.IsNullOrWhiteSpace(msg))
                        {
                            throw new Exception(msg);
                        }
        }


                    if (root.TryGetProperty("errors", out var errorsProp) && errorsProp.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var prop in errorsProp.EnumerateObject())
                        {
                            if (prop.Value.ValueKind == JsonValueKind.Array)
                            {
                                var primerError = prop.Value.EnumerateArray().FirstOrDefault().GetString();
                                if (!string.IsNullOrWhiteSpace(primerError))
        {
                                    throw new Exception(primerError);
                                }
                            }
                        }
                    }
                }

                throw new Exception($"Error HTTP {(int)response.StatusCode}: {response.ReasonPhrase}");
            }
            catch (JsonException)
            {
                throw new Exception($"Error HTTP {(int)response.StatusCode}: {jsonContent}");
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