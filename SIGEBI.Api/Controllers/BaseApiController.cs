using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace SIGEBI.Api.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected int ObtenerUsuarioId()
        {
            if (User?.Identity is null || !User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("El usuario no está autenticado.");

            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(usuarioIdClaim))
                throw new UnauthorizedAccessException("El token no contiene el identificador del usuario.");

            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
                throw new UnauthorizedAccessException("El identificador del usuario en el token no es válido.");

            return usuarioId;
        }

        protected string? ObtenerRol()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }

        protected string? ObtenerCorreo()
        {
            return User.FindFirst(ClaimTypes.Email)?.Value;
        }

        protected string? ObtenerNombre()
        {
            return User.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}