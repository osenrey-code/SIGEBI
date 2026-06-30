

namespace SIGEBI.Application.Interfaces.ext
{
    public interface IServicioToken
    {
        string GenerarToken(
            Guid usuarioId,
            string nombreCompleto,
            string correo,
            string tipoUsuario);
    }
}
