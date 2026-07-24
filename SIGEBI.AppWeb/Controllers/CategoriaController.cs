using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using SIGEBI.AppWeb.Models.Categoria;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public class CategoriaController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoriaController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cliente = _httpClientFactory.CreateClient("API");
            var token = User.FindFirst("Token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var respuesta = await cliente.GetAsync("api/categorias");

            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var categorias = JsonSerializer.Deserialize<List<CategoriaViewModel>>(contenido, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(categorias);
            }

            TempData["Error"] = "No se pudieron cargar las categorías.";
            return View(new List<CategoriaViewModel>());
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(CategoriaViewModel modelo)
        {
            if (!ModelState.IsValid) return View(modelo);

            var cliente = _httpClientFactory.CreateClient("API");
            var token = User.FindFirst("Token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(modelo), Encoding.UTF8, "application/json");
            var respuesta = await cliente.PostAsync("api/categorias/registrar", jsonContent);

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Success"] = "Categoría agregada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error al agregar la categoría.";
            return View(modelo);
        }
    }
}