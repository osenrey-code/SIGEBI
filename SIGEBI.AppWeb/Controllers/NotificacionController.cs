using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize]
    public class NotificacionController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NotificacionController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMisNotificaciones()
        {
            var cliente = _httpClientFactory.CreateClient("API");
            var token = User.FindFirst("Token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var respuesta = await cliente.GetAsync("api/notificacion/consultar");

            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                return Content(contenido, "application/json");
            }

            return Json(new List<object>());
        }
    }
}