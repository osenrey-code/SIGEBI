using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces.Service;

namespace SIGEBI.AppWeb.Controllers
{
    [Authorize]
    public class PrestamosController : BaseController
    {
        private readonly IGestionPrestamos _prestamos;
        private readonly ILogger<PrestamosController> _logger;
        public PrestamosController(IGestionPrestamos prestamos, ILogger<PrestamosController> logger)
        {
            _prestamos = prestamos;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

        }




    }  
}
