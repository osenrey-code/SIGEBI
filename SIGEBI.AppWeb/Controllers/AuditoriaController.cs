using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;
using System.Security.Claims;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Administrador,Auditor")]
    public class AuditoriaController : Controller
    {
        private readonly ILogAuditoria _logAuditoria;

        public AuditoriaController(ILogAuditoria logAuditoria)
        {
            _logAuditoria = logAuditoria;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] ConsultarLogAuditoriaRequest request)
        {
            try
            {
                var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                     ?? User.FindFirst("UsuarioId")?.Value
                                     ?? User.FindFirst("sub")?.Value;

                if (!int.TryParse(usuarioIdClaim, out int usuarioId))
                {
                    TempData["Error"] = "No se pudo identificar al usuario actual.";
                    return RedirectToAction("Index", "Home");
                }

                var logs = await _logAuditoria.ConsultarAuditoriaLog(request ?? new ConsultarLogAuditoriaRequest(), usuarioId);

                ViewBag.Filtros = request;

                return View(logs);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<SIGEBI.Application.DTOs.Response.LogAuditoriaResponse>());
            }
        }
    }
}