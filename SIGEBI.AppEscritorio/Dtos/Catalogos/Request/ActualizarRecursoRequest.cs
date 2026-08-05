namespace SIGEBI.AppEscritorio.Dtos.Catalogo.Request
{
    public class ActualizarRecursoRequest
    {
        public int RecursoBibliograficoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public int AnioPublicado { get; set; }
        public int CantidadEjemplares { get; set; }
        public string? ImagenUrlActual { get; set; }
        public string? RutaNuevaImagenLocal { get; set; }
        public string? Descripcion { get; set; }
    }
}