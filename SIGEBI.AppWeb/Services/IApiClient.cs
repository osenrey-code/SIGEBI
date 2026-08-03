namespace SIGEBI.AppWeb.Services
{
    public interface IApiClient
    {
        Task<T?> GetAsync<T>(string endpoint);
        Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);
        Task PostAsync<TRequest>(string endpoint, TRequest data);
        Task PutAsync<TRequest>(string endpoint, TRequest data);
        Task PatchAsync<TRequest>(string endpoint, TRequest data);
        Task DeleteAsync(string endpoint);
        Task<byte[]> GetByteArrayAsync(string endpoint);
    }
}