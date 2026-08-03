namespace SIGEBI.Application.Interfaces.Service
{
    public interface IStorageService
    {
        Task<string> GuardarAsync(Stream archivoStream,
            string extensionArchivo, string nombreCarpeta = "imagenes"
            );
    }
}
