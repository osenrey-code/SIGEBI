using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.UseCase.Auditory;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/auditoria")]
    public class AuditoriaController : ControllerBase
    {
        private readonly ConsultarLogAuditoria _consultarLogAuditoria;

        public AuditoriaController(
            ConsultarLogAuditoria consultarLogAuditoria)
        {
            _consultarLogAuditoria = consultarLogAuditoria;
        }

        // GET: api/auditoria
        // GET: api/auditoria?usuarioId=2
        // GET: api/auditoria?accion=Registrar Usuario
        // GET: api/auditoria?entidadAfectada=Usuarios
        [HttpGet]
        public async Task<IActionResult> ConsultarRegistros(
            [FromQuery] ConsultarLogAuditoriaRequest request)
        {
            int usuarioEjecutorId = 1;

            var registros = await _consultarLogAuditoria
                .EjecutarAsync(
                    request,
                    usuarioEjecutorId
                );

            return Ok(registros);
        }
    }
}