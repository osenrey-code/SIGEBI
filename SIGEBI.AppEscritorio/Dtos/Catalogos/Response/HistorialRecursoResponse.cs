using System;

namespace SIGEBI.AppEscritorio.Dtos.Catalogo.Response
{
    public class HistorialRecursoResponse
    {
        public int AuditoriaId { get; set; }
        public int RecursoBibliograficoId { get; set; }
        public string TipoCambio { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public int UsuarioResponsableId { get; set; }
    }
}