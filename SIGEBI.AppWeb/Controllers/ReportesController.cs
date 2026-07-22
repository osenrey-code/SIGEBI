using Microsoft.AspNetCore.Mvc;

namespace SIGEBI.AppWeb.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
