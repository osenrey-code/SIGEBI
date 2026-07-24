using Microsoft.AspNetCore.Mvc;
using SIGEBI.Domain.Exceptions;
using System.Security.Claims;

namespace SIGEBI.AppWeb.Controllers
{
    public abstract class BaseController : Controller
    {
        protected int ObtenerUsuarioId()
        {
            var claim = User.FindFirst("UsuarioId")?.Value;
            if (!int.TryParse(claim, out int usuarioId))
                throw new UnauthorizedAccessException("No se pude obtener el usuario autenticado.");

            return usuarioId;
        }

        protected string ObtenerNombreUsuario()
        {
            return User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        }

        protected string ObtenerRol()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        protected string ObtenerIdentificacion()
        {
            return User.FindFirst("Identificacion")?.Value ?? string.Empty;
        }

        protected async Task<IActionResult> EjecutarConsultaAsync<TModel>(
            Func<Task<TModel>> consulta,
            Func<TModel, IActionResult> vistaExito,
            Func<TModel> fallbackModelo,
            string mensajeErrorLog,
            ILogger logger,
            string? nombreVista = null)
        {
            try
            {
                var resultado = await consulta();
                return vistaExito(resultado);
            }
            catch (BusinessException ex)
            {
                logger.LogWarning(ex, "Regla de negocio no cumplida: {Mensaje}", ex.Message);
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{MensajeLog}", mensajeErrorLog);
                TempData["Error"] = "Ocurrió un error inesperado al cargar la información.";
            }

            var modeloVacio = fallbackModelo();
            return string.IsNullOrEmpty(nombreVista) ? View(modeloVacio) : View(nombreVista, modeloVacio);
        }

      
        protected async Task<IActionResult> EjecutarFormularioAsync(
            Func<Task> accion,
            Func<IActionResult> vistaError,
            string accionExito,
            string mensajeExito,
            ILogger logger,
            string? mensajeErrorLog = null)
        {
            if (!ModelState.IsValid)
                return vistaError();

            try
            {
                await accion();
                TempData["Success"] = mensajeExito;
                return RedirectToAction(accionExito);
            }
            catch (BusinessException ex)
            {
                logger.LogWarning(ex, "Advertencia de negocio al procesar formulario: {Mensaje}", ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
                return vistaError();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{MensajeLog}", mensajeErrorLog ?? "Error no controlado al procesar formulario.");
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al procesar la solicitud.");
                return vistaError();
            }
        }
        protected async Task<IActionResult> EjecutarAccionRedireccionAsync(
            Func<Task> accion,
            string accionDestino,
            string mensajeExito,
            ILogger logger,
            object? routeValues = null)
        {
            try
            {
                await accion();
                TempData["Success"] = mensajeExito;
            }
            catch (BusinessException ex)
            {
                logger.LogWarning(ex, "Advertencia de negocio: {Mensaje}", ex.Message);
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error no controlado al ejecutar la acción.");
                TempData["Error"] = "Ocurrió un error inesperado al procesar la solicitud.";
            }

            return routeValues != null
                ? RedirectToAction(accionDestino, routeValues)
                : RedirectToAction(accionDestino);
        }
        protected async Task<IActionResult> EjecutarDescargaPdfAsync(
            Func<Task<byte[]>> generadorPdf,
            string nombreArchivo,
            string accionRedireccionError,
            ILogger logger,
            object? routeValuesError = null)
        {
            try
            {
                var pdfBytes = await generadorPdf();
                return File(pdfBytes, "application/pdf", nombreArchivo);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al generar y descargar el archivo PDF {NombreArchivo}.", nombreArchivo);
                TempData["Error"] = "No se pudo generar el archivo PDF en este momento.";
                return routeValuesError != null
                    ? RedirectToAction(accionRedireccionError, routeValuesError)
                    : RedirectToAction(accionRedireccionError);
            }
        }
    }
}
