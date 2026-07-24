using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces.Service;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SIGEBI.AppWeb.Controllers
{
    public class PenalizacionesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGestionPenalizaciones _gestionPenalizaciones; 

        public PenalizacionesController(IHttpClientFactory httpClientFactory, IGestionPenalizaciones gestionPenalizaciones)
        {
            _httpClientFactory = httpClientFactory;
            _gestionPenalizaciones = gestionPenalizaciones;
        }

        private int ObtenerUsuarioId()
        {
            var idClaim = User.FindFirst("UsuarioId") ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (idClaim != null && int.TryParse(idClaim.Value, out int id))
            {
                return id;
            }
            return 1; 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Bibliotecario")]
        public async Task<IActionResult> Resolver(int penalizacionId, string motivoResolucion)
        {
            try
            {
                var cliente = _httpClientFactory.CreateClient("API");
                var token = User.FindFirst("Token")?.Value;

                if (!string.IsNullOrEmpty(token))
                {
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var request = new ResolverPenalizacionRequest
                {
                    PenalizacionId = penalizacionId,
                    MotivoResolucion = motivoResolucion
                };

                int usuarioId = ObtenerUsuarioId();

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                );

                var respuesta = await cliente.PostAsync($"api/penalizaciones/resolver/{penalizacionId}", jsonContent);

                if (respuesta.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Penalización resuelta correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var errorContenido = await respuesta.Content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(errorContenido))
                    {
                        errorContenido = respuesta.StatusCode.ToString();
                    }
                    TempData["Error"] = $"Error de API: {errorContenido}";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}