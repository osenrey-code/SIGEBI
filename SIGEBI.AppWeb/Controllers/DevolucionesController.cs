using Microsoft.AspNetCore.Mvc;
using SIGEBI.AppWeb.Models;
using System.Diagnostics;

namespace SIGEBI.AppWeb.Controllers
{
    public class DevolucionesController : Controller
    {
        private readonly ILogger<DevolucionesController> _logger;

        public DevolucionesController(ILogger<DevolucionesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new RegistrarUsuarioViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
