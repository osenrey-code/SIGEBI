

namespace SIGEBI.Application.Interfaces.ext
{
    public interface IServicioToken
    {
        string GenerarToken(
            int  usuarioId,
            string nombreCompleto,
            string correo,
            string tipoUsuario);
    }
}
