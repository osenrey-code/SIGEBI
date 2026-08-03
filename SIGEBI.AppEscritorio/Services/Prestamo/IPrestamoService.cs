using SIGEBI.AppEscritorio.Dtos.Prestamos;

namespace SIGEBI.AppEscritorio.Services.Prestamo
{
    public interface IPrestamoService
    {
        Task<List<SolicitudDto>> ConsultarTodasSolicitudesAsync();
        Task<List<SolicitudDto>> ConsultarSolicitudesPendientesAsync();
        Task<SolicitudDto?> ObtenerDetalleSolicitudAsync(int id);
        Task AprobarSolicitudAsync(AprobarSolicitudRequest request);
        Task<List<PrestamoDto>> ConsultarActivosAsync(ConsultarPrestamosActivosRequest request);
        Task<List<PrestamoDto>> ConsultarHistorialAsync(ConsultarHistorialPrestamosRequest request);
    }
}
