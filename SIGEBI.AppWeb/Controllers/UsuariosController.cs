using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;
using SIGEBI.AppWeb.Models.Usuarios;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize] // Permite acceso a usuarios autenticados (para MiPerfil)
    public class UsuariosController : BaseController
    {
        private readonly IGestionUsuariosUseCase _usuarios;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(IGestionUsuariosUseCase usuarios, ILogger<UsuariosController> logger)
        {
            _usuarios = usuarios;
            _logger = logger;
        }

        #region CONSULTA Y LISTADO DE USUARIOS

        [Authorize(Roles = "Administrador,Bibliotecario")]
        [HttpGet]
        public async Task<IActionResult> Index(string? identificacionBusqueda)
        {
            try
            {
                var request = new ConsultarUsuariosRequest
                {
                    Identificacion = identificacionBusqueda
                };
                var respuesta = await _usuarios.ConsultarUsuariosAsync(request);

                if (User.IsInRole("Bibliotecario") && !User.IsInRole("Administrador"))
                {
                    respuesta = respuesta.Where(u =>
                        u.TipoUsuario.Equals("Estudiante", StringComparison.OrdinalIgnoreCase) ||
                        u.TipoUsuario.Equals("Docente", StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                var modelo = new UsuarioIndexViewModel
                {
                    Identificacion = identificacionBusqueda,
                    Usuarios = respuesta.Select(usuario => new UsuarioItemViewModel
                    {
                        UsuarioId = usuario.UsuarioId,
                        Identificacion = usuario.Identificacion,
                        NombreCompleto = usuario.NombreCompleto,
                        Correo = usuario.Correo,
                        TipoUsuario = usuario.TipoUsuario,
                        Estado = usuario.Estado
                    }).ToList()
                };
                return View(modelo);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "No fue posible consultar los usuarios.");
                TempData["Error"] = ex.Message;
                return View(new UsuarioIndexViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar los usuarios.");
                TempData["Error"] = "No fue posible consultar los usuarios.";
                return View(new UsuarioIndexViewModel());
            }
        }

        #endregion

        #region CREACIÓN Y EDICIÓN DE USUARIOS

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Crear()
        {
            return View(new RegistrarUsuarioViewModel());
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(RegistrarUsuarioViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var request = new RegistrarUsuarioRequest
                {
                    Identificacion = model.Identificacion,
                    NombreCompleto = model.NombreCompleto,
                    Correo = model.Correo,
                    Tipo = model.Tipo
                };

                var usuario = await _usuarios.RegistrarUsuarioAsync(request, ObtenerUsuarioId());
                _logger.LogInformation("Usuario registrado correctamente. UsuarioId: {UsuarioId}", usuario.UsuarioId);
                TempData["Success"] = "Usuario registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("No fue posible registrar el usuario, Motivo: {Motivo}", ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar un usuario.");
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado.");
                return View(model);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var usuario = await _usuarios.BuscarPorIdAsync(id);

                var model = new ActualizarUsuarioViewModel
                {
                    UsuarioId = usuario.UsuarioId,
                    Identificacion = usuario.Identificacion,
                    NombreCompleto = usuario.NombreCompleto,
                    Correo = usuario.Correo,
                    Tipo = usuario.TipoUsuario
                };

                return View(model);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("No fue posible obtener el usuario {UsuarioId}. Motivo: {Motivo}", id, ex.Message);
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener el usuario {UsuarioId}", id);
                TempData["Error"] = "Ocurrió un error inesperado.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ActualizarUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var request = new ActualizarUsuarioRequest
                {
                    NombreCompleto = model.NombreCompleto
                };

                var usuario = await _usuarios.ActualizarUsuarioAsync(request, model.UsuarioId, ObtenerUsuarioId());

                _logger.LogInformation("Usuario actualizado correctamente. UsuarioId: {UsuarioId}", usuario.UsuarioId);
                TempData["Success"] = "Usuario actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("No fue posible actualizar el usuario {UsuarioId}. Motivo: {Motivo}", model.UsuarioId, ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar el usuario {UsuarioId}", model.UsuarioId);
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado.");
                return View(model);
            }
        }

        #endregion

        #region ACTIVACIÓN Y DESACTIVACIÓN DE USUARIOS

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Desactivar(int id)
        {
            try
            {
                var usuario = await _usuarios.BuscarPorIdAsync(id);

                var model = new DesactivarUsuarioViewModel
                {
                    UsuarioId = usuario.UsuarioId,
                    NombreCompleto = usuario.NombreCompleto,
                    Identificacion = usuario.Identificacion
                };

                return View(model);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("No fue posible obtener el usuario {UsuarioId} para desactivarlo. Motivo: {Motivo}", id, ex.Message);
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener el usuario {UsuarioId} para desactivarlo.", id);
                TempData["Error"] = "Ocurrió un error inesperado.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desactivar(DesactivarUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var request = new DesactivarUsuarioRequest
                {
                    Motivo = model.Motivo
                };

                await _usuarios.DesactivarUsuarioAsync(request, model.UsuarioId, ObtenerUsuarioId());

                _logger.LogInformation("Usuario desactivado correctamente. UsuarioId: {UsuarioId}", model.UsuarioId);
                TempData["Success"] = "Usuario desactivado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("No fue posible desactivar el usuario {UsuarioId}. Motivo: {Motivo}", model.UsuarioId, ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al desactivar el usuario {UsuarioId}.", model.UsuarioId);
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado.");
                return View(model);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Activar(int id)
        {
            try
            {
                var usuario = await _usuarios.BuscarPorIdAsync(id);

                var model = new ActivarUsuarioViewModel
                {
                    UsuarioId = usuario.UsuarioId,
                    Identificacion = usuario.Identificacion,
                    NombreCompleto = usuario.NombreCompleto
                };

                return View(model);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("No fue posible obtener el usuario {UsuarioId} para activarlo. Motivo: {Motivo}", id, ex.Message);
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener el usuario {UsuarioId}.", id);
                TempData["Error"] = "Ocurrió un error inesperado.";
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activar(ActivarUsuarioViewModel model)
        {
            try
            {
                await _usuarios.ActivarUsuarioAsync(model.UsuarioId, ObtenerUsuarioId());

                _logger.LogInformation("Usuario activado correctamente. UsuarioId: {UsuarioId}", model.UsuarioId);
                TempData["Success"] = "Usuario activado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("No fue posible activar el usuario {UsuarioId}. Motivo: {Motivo}", model.UsuarioId, ex.Message);
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al activar el usuario {UsuarioId}.", model.UsuarioId);
                TempData["Error"] = "Ocurrió un error inesperado.";
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region MI PERFIL Y GESTIÓN DE CONTRASEÑAS

        // GET: Usuarios/MiPerfil
        [HttpGet]
        public async Task<IActionResult> MiPerfil()
        {
            try
            {
                var usuarioId = ObtenerUsuarioId();
                var usuario = await _usuarios.BuscarPorIdAsync(usuarioId);
                ViewBag.Usuario = usuario;

                return View(new CambiarPasswordRequest());
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("No fue posible cargar el perfil del usuario. Motivo: {Motivo}", ex.Message);
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
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
        public async Task<IActionResult> CambiarMiPassword(CambiarPasswordRequest request)
        {
            var usuarioId = ObtenerUsuarioId();

            if (!ModelState.IsValid)
            {
                try { ViewBag.Usuario = await _usuarios.BuscarPorIdAsync(usuarioId); } catch { }
                return View("MiPerfil", request);
            }

            try
            {
                await _usuarios.CambiarPasswordAsync(request, usuarioId);

                _logger.LogInformation("El usuario {UsuarioId} cambió su contraseña correctamente.", usuarioId);
                TempData["Success"] = "Tu contraseña ha sido actualizada correctamente.";
                return RedirectToAction(nameof(MiPerfil));
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("No fue posible cambiar la contraseña del usuario {UsuarioId}. Motivo: {Motivo}", usuarioId, ex.Message);
                TempData["Error"] = ex.Message;

                try { ViewBag.Usuario = await _usuarios.BuscarPorIdAsync(usuarioId); } catch { }
                return View("MiPerfil", request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al cambiar la contraseña del usuario {UsuarioId}.", usuarioId);
                TempData["Error"] = "Ocurrió un error inesperado al intentar cambiar la contraseña.";

                try { ViewBag.Usuario = await _usuarios.BuscarPorIdAsync(usuarioId); } catch { }
                return View("MiPerfil", request);
            }
        }

        // POST: Usuarios/ResetearPasswordAdmin (Acción del Administrador)
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetearPasswordAdmin(int usuarioId, string nuevaPassword)
        {
            var actorId = ObtenerUsuarioId();

            try
            {
                await _usuarios.CambiarPasswordAdminAsync(usuarioId, nuevaPassword, actorId);

                _logger.LogInformation("El administrador {ActorId} restableció la contraseña del usuario {UsuarioId}.", actorId, usuarioId);
                TempData["Success"] = "La contraseña del usuario fue restablecida exitosamente.";
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning("No fue posible restablecer la contraseña del usuario {UsuarioId}. Motivo: {Motivo}", usuarioId, ex.Message);
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al restablecer la contraseña del usuario {UsuarioId}.", usuarioId);
                TempData["Error"] = "Ocurrió un error inesperado al restablecer la contraseña.";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}