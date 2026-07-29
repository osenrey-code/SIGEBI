//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using SIGEBI.Application.DTOs.Request;
//using SIGEBI.Application.Interfaces.Service;
//using SIGEBI.AppWeb.Models.Usuarios;
//using SIGEBI.Domain.Exceptions;

//namespace SIGEBI.AppWeb.Controllers
//{
//    [Authorize] // Permite acceso a usuarios autenticados (para MiPerfil)
//    public class UsuariosController : BaseController
//    {
//        private readonly IGestionUsuariosUseCase _usuarios;
//        private readonly ILogger<UsuariosController> _logger;

//        public UsuariosController(IGestionUsuariosUseCase usuarios, ILogger<UsuariosController> logger)
//        {
//            _usuarios = usuarios;
//            _logger = logger;
//        }

//        // GET: Usuarios/MiPerfil
//        [HttpGet]
//        public async Task<IActionResult> MiPerfil()
//        {
//            try
//            {
//                var usuarioId = ObtenerUsuarioId();
//                var usuario = await _usuarios.BuscarPorIdAsync(usuarioId);
//                ViewBag.Usuario = usuario;

//                return View(new CambiarPasswordRequest());
//            }
//            catch (BusinessException ex)
//            {
//                _logger.LogWarning("No fue posible cargar el perfil del usuario. Motivo: {Motivo}", ex.Message);
//                TempData["Error"] = ex.Message;
//                return RedirectToAction("Index", "Home");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error inesperado al cargar el perfil de usuario.");
//                TempData["Error"] = "Ocurrió un error inesperado al cargar el perfil.";
//                return RedirectToAction("Index", "Home");
//            }
//        }

//        // POST: Usuarios/CambiarMiPassword (Autoservicio)
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> CambiarMiPassword(CambiarPasswordRequest request)
//        {
//            var usuarioId = ObtenerUsuarioId();

//            if (!ModelState.IsValid)
//            {
//                try { ViewBag.Usuario = await _usuarios.BuscarPorIdAsync(usuarioId); } catch { }
//                return View("MiPerfil", request);
//            }

//            try
//            {
//                await _usuarios.CambiarPasswordAsync(request, usuarioId);

//                _logger.LogInformation("El usuario {UsuarioId} cambió su contraseña correctamente.", usuarioId);
//                TempData["Success"] = "Tu contraseña ha sido actualizada correctamente.";
//                return RedirectToAction(nameof(MiPerfil));
//            }
//            catch (BusinessException ex)
//            {
//                _logger.LogWarning("No fue posible cambiar la contraseña del usuario {UsuarioId}. Motivo: {Motivo}", usuarioId, ex.Message);
//                TempData["Error"] = ex.Message;

//                try { ViewBag.Usuario = await _usuarios.BuscarPorIdAsync(usuarioId); } catch { }
//                return View("MiPerfil", request);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error inesperado al cambiar la contraseña del usuario {UsuarioId}.", usuarioId);
//                TempData["Error"] = "Ocurrió un error inesperado al intentar cambiar la contraseña.";

//                try { ViewBag.Usuario = await _usuarios.BuscarPorIdAsync(usuarioId); } catch { }
//                return View("MiPerfil", request);
//            }
//        }
//    }
//}