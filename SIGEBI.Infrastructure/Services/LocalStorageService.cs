using Microsoft.AspNetCore.Hosting;
using SIGEBI.Application.Interfaces.ext;

namespace SIGEBI.Infrastructure.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly IWebHostEnvironment _env;


        public LocalStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> GuardarAsync(Stream archivoStream, string extensionArchivo, string nombreCarpeta = "imagenes")
        {
            if (archivoStream == null || archivoStream.Length == 0) 
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(extensionArchivo))
            {
                return string.Empty;
            }

            extensionArchivo = extensionArchivo.ToLower();

            var extensionesPermitidas = new[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            };

            if (!extensionesPermitidas.Contains(extensionArchivo))
            {
                throw new Exception("Formato de imagen no permitido.");
            }

            var nombreArchivo = $"{Guid.NewGuid()}{extensionArchivo}";
            var webRootPath = _env.WebRootPath;

            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(_env.ContentRootPath, "wwwroot");
            }

            var carpetaDestino = Path.Combine(webRootPath, nombreCarpeta);

            if (!Directory.Exists(carpetaDestino))
            {
                Directory.CreateDirectory(carpetaDestino);
            }

            var rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

            if (archivoStream.CanSeek)
            {
                archivoStream.Position = 0;
            }

            using var fileStream = new FileStream(rutaCompleta, FileMode.Create);

            await archivoStream.CopyToAsync(fileStream);
            return $"/{nombreCarpeta}/{nombreArchivo}";
        }
    }
}
