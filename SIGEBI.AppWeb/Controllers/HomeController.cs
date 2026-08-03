using Microsoft.AspNetCore.Mvc;

namespace SIGEBI.AppWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToAction("Login", "Autenticacion");
            }
            return View();
        }
    }
}
