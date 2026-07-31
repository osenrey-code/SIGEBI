namespace SIGEBI.AppEscritorio.Dtos.Catalogo.Response
{
    public class EjemplarItemResponse
    {
        public int EjemplarId { get; set; }
        public string Identificador { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? Observacion { get; set; }
    }
}