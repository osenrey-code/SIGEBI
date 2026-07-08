using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class ConsultarHistorialRecurso
    {
        private readonly IRepositorioRecurso _recursos;
        private readonly IRepositorioAuditoria _auditoria;

        public ConsultarHistorialRecurso(
            IRepositorioRecurso recursos,
            IRepositorioAuditoria auditoria)
        {
            _recursos = recursos;
            _auditoria = auditoria;
        }

        public async Task<IEnumerable<HistorialRecursoResponse>> EjecutarAsync(
            ConsultarHistorialRecursoRequest request)
        {
            if (request.RecursoBibliograficoId <= 0)
                throw new BusinessException("El recurso es obligatorio.");

            var recurso = await _recursos.ObtenerporIdAsync(request.RecursoBibliograficoId);

            if (recurso is null)
                throw new BusinessException("El recurso no existe.");

            var registros = await _auditoria.ObtenerPorEntidadAsync("RecursoBibliografico");

            var historial = registros
                .Where(r => r.Detalle.Contains($"ID {request.RecursoBibliograficoId}"))
                .OrderByDescending(r => r.FechaRegistro)
                .Select(r => new HistorialRecursoResponse
                {
                    AuditoriaId = r.AuditoriaId,
                    RecursoBibliograficoId = request.RecursoBibliograficoId,
                    TipoCambio = r.Accion,
                    Detalle = r.Detalle,
                    FechaRegistro = r.FechaRegistro,
                    UsuarioResponsableId = r.UsuarioId
                })
                .ToList();

            return historial;
        }
    }
}