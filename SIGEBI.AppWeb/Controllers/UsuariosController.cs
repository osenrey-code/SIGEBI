using SIGEBI.Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.AppWeb.Models.Usuarios;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public class UsuariosController : BaseController
    {
        private readonly IGestionUsuariosUseCase _usuarios;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(IGestionUsuariosUseCase usuarios, ILogger<UsuariosController> logger)
        {
            _usuarios = usuarios;
            _logger = logger;
        }

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
                _logger.LogWarning(ex,
                    "No fue posible consultar los usuarios.");

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
            catch(BusinessException ex)
            {
                _logger.LogWarning("No fue posible obtener el usuario {Usuarioid}. Motivo: {Motivo}", id, ex.Message);
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener el usuario {usuarioId}", id);
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
                _logger.LogWarning("No fue posible actualizar el usuario {Usuarioid}. Motivo: {Motivo}", model.UsuarioId, ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inespedado al actualizar el usuario {UsuarioId}", model.UsuarioId);
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado.");
                return View(model);
            }
        }

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
                _logger.LogWarning(
                    "No fue posible obtener el usuario {UsuarioId} para desactivarlo. Motivo: {Motivo}",
                    id,
                    ex.Message);

                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error inesperado al obtener el usuario {UsuarioId} para desactivarlo.",
                    id);

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

                await _usuarios.DesactivarUsuarioAsync(
                    request,
                    model.UsuarioId,
                    ObtenerUsuarioId());

                _logger.LogInformation(
                    "Usuario desactivado correctamente. UsuarioId: {UsuarioId}",
                    model.UsuarioId);

                TempData["Success"] = "Usuario desactivado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(
                    "No fue posible desactivar el usuario {UsuarioId}. Motivo: {Motivo}",
                    model.UsuarioId,
                    ex.Message);

                ModelState.AddModelError(string.Empty, ex.Message);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error inesperado al desactivar el usuario {UsuarioId}.",
                    model.UsuarioId);

                ModelState.AddModelError(
                    string.Empty,
                    "Ocurrió un error inesperado.");

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
                _logger.LogWarning(
                    "No fue posible obtener el usuario {UsuarioId} para activarlo. Motivo: {Motivo}",
                    id,
                    ex.Message);

                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error inesperado al obtener el usuario {UsuarioId}.",
                    id);

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
                await _usuarios.ActivarUsuarioAsync(
                    model.UsuarioId,
                    ObtenerUsuarioId());

                _logger.LogInformation(
                    "Usuario activado correctamente. UsuarioId: {UsuarioId}",
                    model.UsuarioId);

                TempData["Success"] = "Usuario activado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(
                    "No fue posible activar el usuario {UsuarioId}. Motivo: {Motivo}",
                    model.UsuarioId,
                    ex.Message);

                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error inesperado al activar el usuario {UsuarioId}.",
                    model.UsuarioId);

                TempData["Error"] = "Ocurrió un error inesperado.";

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
