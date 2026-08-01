using SIGEBI.AppEscritorio.Dtos.Auditorias;

namespace SIGEBI.AppEscritorio.Services.Auditoria
{
    public interface IAuditoriaService
    {
        Task<List<LogAuditoriaResponseDto>> ConsultarLogsAsync(ConsultarLogAuditoriaRequestDto request);
    }
}
