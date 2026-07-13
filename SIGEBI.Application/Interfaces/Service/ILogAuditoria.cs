
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;

namespace SIGEBI.Application.Interfaces.Service
{
    public interface ILogAuditoria
    {
        Task<IEnumerable<LogAuditoriaResponse>> ConsultarAuditoriaLog(ConsultarLogAuditoriaRequest request, int usuarioId);
    }
}
