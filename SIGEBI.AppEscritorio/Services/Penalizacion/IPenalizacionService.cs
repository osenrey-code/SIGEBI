using SIGEBI.AppEscritorio.Dtos.Penalizaciones;

namespace SIGEBI.AppEscritorio.Services.Penalizaciones
{
    public interface IPenalizacionService
    {
        Task<List<PenalizacionDto>> ConsultarPenalizacionesAsync(ConsultarPenalizacionesRequestDto request);
        Task ResolverPenalizacionAsync(ResolverPenalizacionRequestDto request);
    }



}