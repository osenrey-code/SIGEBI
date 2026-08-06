namespace SIGEBI.AppEscritorio.Dtos.Prestamos
{
    public class RechazarSolicitudRequest
    {
        public int SolicitudId { get; set; }
        public string MotivoRechazo { get; set; } = string.Empty;
    }
}