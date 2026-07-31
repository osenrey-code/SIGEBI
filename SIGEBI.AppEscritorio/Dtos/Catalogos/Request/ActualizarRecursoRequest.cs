namespace SIGEBI.AppEscritorio.Dtos.Catalogo.Request
{
    public class ActualizarRecursoRequest
    {
        public int RecursoBibliograficoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public int AnioPublicado { get; set; }

        public string? ImagenUrlActual { get; set; }

        // Exclusivo de WinForms: Ruta local del nuevo archivo a subir
        public string? RutaNuevaImagenLocal { get; set; }
    }
}