using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.ext;
using SIGEBI.Application.Interfaces.Repositories;

namespace SIGEBI.Application.UseCase.Catalogo
{
    public class ConsultarHistorialRecurso
    {
        private readonly IRepositorioRecurso _recursos;
        private readonly IRepositorioAuditoria _auditoria;

        public ConsultarHistorialRecurso(IRepositorioRecurso recursos, IRepositorioAuditoria auditoria)
        {
            _recursos = recursos;
            _auditoria = auditoria;
        }

        public async Task<ResultadoOperacionResponse<IEnumerable<HistorialRecursoResponse>>> EjecutarAsync(
            ConsultarHistorialRecursoRequest request)
        {
            // Validamos que venga el recurso que se quiere consultar.
            if (request.RecursoId == Guid.Empty)
            {
                return ResultadoOperacionResponse<IEnumerable<HistorialRecursoResponse>>.Error(
                    "El recurso es obligatorio."
                );
            }

            // Verificamos que el recurso exista antes de consultar su historial.
            var recurso = await _recursos.ObtenerporIdAsync(request.RecursoId);

            if (recurso is null)
            {
                return ResultadoOperacionResponse<IEnumerable<HistorialRecursoResponse>>.Error(
                    "El recurso no existe."
                );
            }

            // Consultamos los registros de auditoría relacionados con recursos bibliográficos.
            // Aquí no usamos DbContext directamente, solo el contrato IRepositorioAuditoria.
            var registros = await _auditoria.ConsultarAsync(
                usuarioId: null,
                accion: null,
                entidadAfectada: "RecursoBibliografico",
                fechaInicio: null,
                fechaFin: null
            );

            // Filtramos solamente los registros que pertenecen al recurso solicitado.
            var historial = registros
                .Where(r => r.EntidadId == request.RecursoId)
                .OrderByDescending(r => r.FechaRegistro)
                .Select(r => new HistorialRecursoResponse
                {
                    Id = r.Id,
                    RecursoId = request.RecursoId,

                    // Ejemplo: Registrar recurso, Actualizar recurso, Cambiar estado de recurso.
                    TipoCambio = r.Accion,

                    // En actualización/cambio de estado aquí aparece cómo estaba antes.
                    EstadoAnterior = r.ValoresAnteriores,

                    // Aquí aparece cómo quedó después.
                    EstadoNuevo = r.ValoresNuevos,

                    FechaRegistro = r.FechaRegistro,
                    UsuarioResponsableId = r.UsuarioId,
                    Responsable = r.Usuario,
                    Detalle = r.Detalle
                })
                .ToList();

            return ResultadoOperacionResponse<IEnumerable<HistorialRecursoResponse>>.Ok(
                "Historial del recurso consultado correctamente.",
                historial
            );
        
        }
    }
}