using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/auditoria")]
    [Authorize]
    public class AuditoriaController : BaseApiController
    {
        private readonly ILogAuditoria _log;

        public AuditoriaController(ILogAuditoria log)
        {
            _log = log;
        }


        [HttpGet("consultar")]
        [Authorize(Roles = "Administrador,Auditor")]
        public async Task<IActionResult> ConsultarRegistros(
            [FromQuery] ConsultarLogAuditoriaRequest request)
        {
            int usuarioEjecutorId = ObtenerUsuarioId();
            var registro = _log.ConsultarAuditoriaLog(request, usuarioEjecutorId);
            return Ok(registro);
        }
    }
}