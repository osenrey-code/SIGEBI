using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Common;
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
            Guard.NotNull(request, "Los datos de consulta del historial");

            if (request.RecursoBibliograficoId <= 0)
                throw new BusinessException("El recurso es obligatorio.");

            var recurso = await _recursos.ObtenerporIdAsync(
                request.RecursoBibliograficoId
            );

            if (recurso is null)
                throw new BusinessException("El recurso no existe.");

            var registros = await _auditoria.ObtenerPorEntidadAsync(
                "RecursosBibliograficos"
            );

            string filtroId = $"ID {request.RecursoBibliograficoId}";

            var historial = registros
                .Where(r =>
                    !string.IsNullOrWhiteSpace(r.Detalle) &&
                    r.Detalle.Contains(filtroId, StringComparison.OrdinalIgnoreCase))
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