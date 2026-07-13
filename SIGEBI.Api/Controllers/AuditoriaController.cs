using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/auditoria")]
    public class AuditoriaController : ControllerBase
    {
        private readonly ILogAuditoria _log;

        public AuditoriaController(ILogAuditoria log)
        {
            _log = log;
        }


        [HttpGet("consultar")]
        public async Task<IActionResult> ConsultarRegistros(
            [FromQuery] ConsultarLogAuditoriaRequest request)
        {
            int usuarioEjecutorId = 1;
            var registro = _log.ConsultarAuditoriaLog(request, usuarioEjecutorId);
            return Ok(registro);
        }
    }
}