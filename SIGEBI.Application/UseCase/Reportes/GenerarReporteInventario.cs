using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Enums;


namespace SIGEBI.Application.UseCase.Reportes
{
    public class GenerarReporteInventario
    {
        private readonly IRepositorioRecurso _recursos;

        public GenerarReporteInventario(IRepositorioRecurso recursos)
        {
            _recursos = recursos;
        }

        public async Task<IEnumerable<ReporteInventarioResponse>> EjecutarAsync()
        {
            return await _recursos.ObtenerReporteInventarioAsync();
        }
    }
}
