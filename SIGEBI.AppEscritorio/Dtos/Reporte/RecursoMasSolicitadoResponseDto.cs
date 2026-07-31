
namespace SIGEBI.AppEscritorio.Dtos.Reporte
{
    public class RecursoMasSolicitadoResponseDto
    {
        public int RecursoBibliograficoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int CantidadSolicitudes { get; set; }
    }
}
