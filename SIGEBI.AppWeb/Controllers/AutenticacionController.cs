using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.AppWeb.Models.DTOs.Auth;
using SIGEBI.AppWeb.Models.DTOs.Usuarios;
using SIGEBI.AppWeb.Models.ViewModels.Auth;
using SIGEBI.AppWeb.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SIGEBI.AppWeb.Controllers
{
    public class AutenticacionController : Controller
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<AutenticacionController> _logger;

        public AutenticacionController(IApiClient apiClient, ILogger<AutenticacionController> logger)
        {
            _apiClient = apiClient;
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
                var request = new LoginRequestdto
                {
                    Identificacion = model.Identificacion.Trim(),
                    Password = model.Password
                };

                var tokenJwt = await _apiClient.PostAsync<LoginRequestdto, LoginResponse>("api/account/login", request);

                if (tokenJwt == null || string.IsNullOrWhiteSpace(tokenJwt.Token))
                {
                    _logger.LogWarning("La API retornó un token nulo o vació para la identificación: {Identificacion}", model.Identificacion);
                    ModelState.AddModelError(string.Empty, "Credencialies inválidas.");
                    return View(model);
                }

                var token = tokenJwt.Token;

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims.ToList();

                claims.Add(new Claim("Token", token));

                var rol = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value;

                var esLector = string.Equals(rol, "Estudiante", StringComparison.OrdinalIgnoreCase) ||
                               string.Equals(rol, "Docente", StringComparison.OrdinalIgnoreCase);


                if (!esLector)
                {
                    _logger.LogWarning("Acceso denegado en Web para {Identificacion}. Rol detectado: '{Rol}'. Este portal es exclusivo para lectores.",
                        model.Identificacion, rol);

                    ModelState.AddModelError(string.Empty, "Acceso denegado: Este portal está habilitado únicamente para Estudiantes y Docentes. " +
                        "Si es personal administrativo, utilice la App de Escritorio.");
                    return View(model);
                }

                var authProperties = new AuthenticationProperties();
                authProperties.StoreTokens(new[]
                {
                    new AuthenticationToken { Name = "access_token", Value = token}
                });

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity); ;

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
                _logger.LogInformation("Inicio de sesión exitoso para {Identificacion} con rol: '{}'.", model.Identificacion, rol);
                
                return RedirectToAction("Index", "Home");
                
            }
            catch (Exception ex)
            {
                
                    _logger.LogError(ex, "Error inesperado durante el login para la identificación {Identificacion}",
                        model.Identificacion);
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(model);
                
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Registro()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegistrarUsuarioRequestDto());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistrarUsuarioRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _apiClient.PostAsync<RegistrarUsuarioRequestDto, object>("api/account/registro", model);

                _logger.LogInformation("Usuario registrado exitosamente con identificación: {Identificacion}", model.Identificacion);
                TempData["Success"] = "¡Registro exitoso! Ya puedes iniciar sesión con tus credenciales.";

                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el registro para la identificación: {Identificacion}", model.Identificacion);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var usuario = User.Identity?.Name ?? "Usuario Desconocido";
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("El usuario {Usuario} cerró sesión correctamente.", usuario);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            _logger.LogWarning("Acceso denegado a un recurso protegido.");
            return View();
        }
    }
}
