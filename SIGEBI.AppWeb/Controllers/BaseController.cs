using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SIGEBI.AppWeb.Controllers
{
    public abstract class BaseController : Controller
    {
        protected int ObtenerUsuarioId()
        {
            var claim = User.FindFirst("UsuarioId")?.Value;
            if (!int.TryParse(claim, out int usuarioId))
                throw new UnauthorizedAccessException("No se pude obtener el usuario autenticado.");

            return usuarioId;
        }

        protected string ObtenerNombreUsuario()
        {
            return User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        }

        protected string ObtenerRol()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        protected string ObtenerIdentificacion()
        {
            return User.FindFirst("Identificacion")?.Value ?? string.Empty;
        }
    }
}
