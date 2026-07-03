
namespace SIGEBI.Application.Interfaces.ext
{
    public interface IServicioPassword
    {
        string GenerarHash(string password);

        bool VerificarPassword(
            string password,
            string passwordHash);
    }
}
