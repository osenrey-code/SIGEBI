using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.AppEscritorio.Services.Api
{
    public interface IApiClient 
    {
        Task<T?> GetTAsync<T>(string endpoint);
        Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);
        Task PostAsync<TRequest>(string endpoint, TRequest data);
        Task PutAsync<TRequest>(string endpoint, TRequest data);
        Task PatchAsync<TRequest>(string endpoint, TRequest data);
        Task DeleteAsync(string endpoint);
        Task<byte[]> GetByteArrayAsync(string endpoint);
    }
}
