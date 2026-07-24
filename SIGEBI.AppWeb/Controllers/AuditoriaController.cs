using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.AppWeb.Models.Auditoria;
using System.Net.Http.Json;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize(Roles = "Administrador,Auditor")]
    public class AuditoriaController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuditoriaController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SIGEBI.Api");
        }

        public async Task<IActionResult> Index()
        {
            List<AuditoriaViewModel> lista = new();
            try
            {
                var response = await _httpClient.GetAsync("api/auditoria/consultar");
                if (response.IsSuccessStatusCode)
                {
                    lista = await response.Content.ReadFromJsonAsync<List<AuditoriaViewModel>>() ?? new();
                }
            }
            catch (Exception)
            {
               
            }

            return View(lista);
        }
    }
}