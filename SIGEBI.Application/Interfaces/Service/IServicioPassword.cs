namespace SIGEBI.Application.Interfaces.Service
{
    public interface IServicioPassword
    {
        string GenerarHash(string password);

        bool VerificarPassword(string password, string passwordHash);
    }
}
