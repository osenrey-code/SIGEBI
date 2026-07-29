//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using SIGEBI.Application.Interfaces.Service;
//using System.Security.Claims;

//namespace SIGEBI.AppWeb.Controllers
//{
//    [Authorize]
//    public class NotificacionController : Controller
//    {
//        private readonly IServicioNotificacion _servicioNotificacion;

//        public NotificacionController(IServicioNotificacion servicioNotificacion)
//        {
//            _servicioNotificacion = servicioNotificacion;
//        }

//        [HttpGet]
//        public async Task<IActionResult> ObtenerMisNotificaciones()
//        {
//            try
//            {
//                var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
//                                     ?? User.FindFirst("UsuarioId")?.Value
//                                     ?? User.FindFirst("sub")?.Value;

//                if (!int.TryParse(usuarioIdClaim, out int usuarioId))
//                {
//                    return Json(new List<object>());
//                }

//                var notificaciones = await _servicioNotificacion.ObtenerPendientesAsync(usuarioId);

//                return Json(notificaciones);
//            }
//            catch
//            {
//                return Json(new List<object>());
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> MarcarLeida(int id)
//        {
//            try
//            {
//                await _servicioNotificacion.MarcarComoLeidaAsync(id);
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//    }
//}