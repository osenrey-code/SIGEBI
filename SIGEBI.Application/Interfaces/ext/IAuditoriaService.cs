
namespace SIGEBI.Application.Interfaces.ext
{
    public interface IAuditoriaService
    {
        Task RegistrarAsync(
           Guid? usuarioId,
           string accion,
           string entidadAfectada,
           Guid? entidadId,
           string resultado,
           string detalle,
           string valoresAnteriores = "",
           string valoresNuevos = "");
    }
}
