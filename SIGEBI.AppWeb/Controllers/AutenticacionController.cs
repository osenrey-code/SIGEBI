using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SIGEBI.AppWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SIGEBI.AppWeb.Controllers
{
    public class AutenticacionController : Controller
    {
        private readonly ILogin _loginService;
        private readonly ILogger<AutenticacionController> _logger;

        public AutenticacionController(ILogin loginService, ILogger<AutenticacionController> logger)
        {
            _loginService = loginService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                _logger.LogInformation("Usuario autenticado intentó acceder nuevamente a login.");
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Intento de login con modelo inválido.");
                return View(model);
            }

            try
            {
                var request = new LoginRequest
                {
                    Identificacion = model.Identificacion.Trim(),
                    Password = model.Password
                };

                var response = await _loginService.AutenticarAsync(request);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, response.UsuarioId.ToString()),
                    new Claim("UsuarioId", response.UsuarioId.ToString()),
                    new Claim(ClaimTypes.Name, response.NombreCompleto),
                    new Claim(ClaimTypes.Role, response.TipoUsuario),
                    new Claim("Identificacion", response.Identificacion),
                    new Claim(ClaimTypes.Email, response.Correo),
                    new Claim("Token", response.Token)
                };

                var identity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                _logger.LogInformation("Login exitoso. UsuarioId: {UsuarioId}, Rol: {TipoUsuario}",
                    response.UsuarioId, response.TipoUsuario);

                return RedirectToAction("Index", "Home");
            }

            catch (BusinessException ex)
            {
                _logger.LogWarning("Login fallido para identificación {Identificacion}. Motivo: {Motivo}",
                    model.Identificacion, ex.Message);

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                
                    _logger.LogError(ex, "Error inesperado durante el login para la identificación {Identificacion}",
                        model.Identificacion);
                    ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al iniciar sesión.");
                    return View(model);
                
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var usuarioId = User.FindFirst("UsuarioId")?.Value;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("Logout ejecutado. Usuario: {UsuarioId}", usuarioId);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            _logger.LogWarning("Acceso denegado a un recurso protegido.");
            return View();
        }
    }
}
