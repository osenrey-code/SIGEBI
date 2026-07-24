using Microsoft.AspNetCore.Mvc;

public class CatalogoController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}