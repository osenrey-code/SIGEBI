namespace SIGEBI.Application.Interfaces.ext
{
    public interface IServicioCorreo
    {
        Task EnviarAsync(string destinatario, string asunto, string mensaje);
    }
}