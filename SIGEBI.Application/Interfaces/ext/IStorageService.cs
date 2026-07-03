namespace SIGEBI.Application.Interfaces.ext
{
    public interface IStorageService
    {
        Task<string> GuardarAsync(Stream archivoStream,
            string extensionArchivo, string nombreCarpeta = "imagenes"
            );
    }
}
