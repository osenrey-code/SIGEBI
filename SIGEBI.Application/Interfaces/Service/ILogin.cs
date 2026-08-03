

using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.Application.Interfaces.Service
{
    public interface ILogin
    {
        Task<LoginResponse> AutenticarAsync(LoginRequest request);
    }
}
