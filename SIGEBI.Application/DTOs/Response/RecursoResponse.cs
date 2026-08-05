namespace SIGEBI.Application.DTOs.Response;


    public class EjemplarItemResponse
    {
        public int EjemplarId { get; set; }
        public string Identificador { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? Observacion { get; set; }
    }

    public class RecursoResponse
    {
        public int RecursoBibliograficoId { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public int AnioPublicado { get; set; }
        public string? ImagenUrl { get; set; }
        public int TotalEjemplares { get; set; }
        public int CopiasDisponibles { get; set; }
        public int? EjemplarDisponibleId { get; set; }
        public string? Descripcion { get; set; }
}