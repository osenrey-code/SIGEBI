using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.AppWeb.Models.DTOs.Usuarios;
using SIGEBI.AppWeb.Models.ViewModels.Usuarios;
using SIGEBI.AppWeb.Services;


namespace SIGEBI.AppWeb.Controllers
{
    [Authorize] // Permite acceso a usuarios autenticados (para MiPerfil)
    public class UsuariosController : BaseController
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(IApiClient apiClient, ILogger<UsuariosController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        // GET: Usuarios/MiPerfil
        [HttpGet]
        public async Task<IActionResult> MiPerfil()
        {
            try
            {
                var usuario = await _apiClient.GetAsync<UsuarioDto>("api/usuarios/perfil");

                if (usuario == null)
                {
                    _logger.LogWarning("No se pudo obtener el perfil del usuario autenticado.");
                    TempData["Error"] = "No se pudieron cargar los datos del perfil.";
                    return RedirectToAction("Index", "Home");
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al cargar el perfil de usuario.");
                TempData["Error"] = "Ocurrió un error inesperado al cargar el perfil.";
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Usuarios/CambiarMiPassword (Autoservicio)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarMiPassword(CambiarPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var primerError = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault() ?? "Por favor, completa correctamente todos los campos.";

                TempData["Error"] = primerError;

                var usuarioActual = await ObtenerPerfilAuxiliar();
                return View("MiPerfil", usuarioActual);
            }

            try
            {
                var request = new CambiarPasswordRequest
                {
                    PasswordActual = model.PasswordActual,
                    PasswordNueva = model.PasswordNueva,
                    ConfirmarPasswordNueva = model.ConfirmarPassword,
                };

                await _apiClient.PatchAsync<CambiarPasswordRequest>("api/usuarios/cambiar-mi-password", request);

                _logger.LogInformation("El usuario cambió su contraseña exitosamente.");
                TempData["Success"] = "Tu contraseña ha sido actualizada correctamente.";
                return RedirectToAction(nameof(MiPerfil));

            } catch(Exception ex)
            {
                _logger.LogError(ex, "Error al intentar cambiar la contraseña.");
                ModelState.AddModelError(string.Empty, ex.Message);

                var usuarioActual = await _apiClient.GetAsync<UsuarioDto>("api/usuarios/perfil");
                return View("MiPerfil", usuarioActual);
            }
        }

        private async Task<UsuarioDto> ObtenerPerfilAuxiliar()
        {
            try
            {
                return await _apiClient.GetAsync<UsuarioDto>("api/usuarios/perfil") ?? new UsuarioDto();
            }
            catch
            {
                return new UsuarioDto();
            }
        }
    }
}