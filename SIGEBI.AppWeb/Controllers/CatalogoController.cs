using Microsoft.AspNetCore.Mvc;
using SIGEBI.AppWeb.Models.Catalogo;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SIGEBI.AppWeb.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CatalogoController(IHttpClientFactory httpClientFactory)
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
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var respuesta = await cliente.GetAsync("api/catalogo/todos");

            if (respuesta.IsSuccessStatusCode)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var recursos = JsonSerializer.Deserialize<IEnumerable<SIGEBI.Application.DTOs.Response.RecursoResponse>>(jsonString, opciones);

                return View(recursos);
            }

            return View(new List<SIGEBI.Application.DTOs.Response.RecursoResponse>());
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new CatalogoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CatalogoViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            using var formContent = new MultipartFormDataContent();

            formContent.Add(new StringContent(modelo.Titulo ?? string.Empty), "Titulo");
            formContent.Add(new StringContent(modelo.Autor ?? string.Empty), "Autor");
            formContent.Add(new StringContent(modelo.ISBN ?? string.Empty), "ISBN");
            formContent.Add(new StringContent(modelo.AnioPublicacion.ToString()), "AnioPublicado");
            formContent.Add(new StringContent(modelo.StockDisponible.ToString()), "CantidadEjemplares");
            formContent.Add(new StringContent(modelo.CategoriaId.ToString()), "CategoriaId");

            if (modelo.ImagenArchivo != null && modelo.ImagenArchivo.Length > 0)
            {
                var streamContent = new StreamContent(modelo.ImagenArchivo.OpenReadStream());
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(modelo.ImagenArchivo.ContentType);

                formContent.Add(streamContent, "ImagenArchivo", modelo.ImagenArchivo.FileName);
            }

            var cliente = _httpClientFactory.CreateClient("API");
            var token = User.FindFirst("Token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var respuesta = await cliente.PostAsync("api/catalogo/registrar", formContent);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errorContenido = await respuesta.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(errorContenido))
                {
                    errorContenido = respuesta.StatusCode.ToString();
                }

                ModelState.AddModelError(string.Empty, $"Error de API: {errorContenido}");
                return View(modelo);
            }
        }
    }
}