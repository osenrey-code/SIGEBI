using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    [Route("api/auditoria")]
    [Authorize(Roles = "Administrador,Auditor")]
    public class AuditoriaController : BaseApiController
    {
        private readonly ILogAuditoria _logAuditoria;

        public AuditoriaController(ILogAuditoria logAuditoria)
        {
            _logAuditoria = logAuditoria;
        }

        [HttpGet("consultar")]
        public async Task<IActionResult> ConsultarAuditoria([FromQuery] ConsultarLogAuditoriaRequest request)
        {
            int usuarioId = ObtenerUsuarioId();
            var resultado = await _logAuditoria.ConsultarAuditoriaLog(request, usuarioId);
            return Ok(resultado);
        }
    }
}