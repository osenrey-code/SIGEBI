namespace SIGEBI.AppEscritorio.Dtos.Catalogo.Request
{
    public class RegistrarRecursoRequest
    {
        public string ISBN { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public int AnioPublicado { get; set; }
        public int CantidadEjemplares { get; set; } = 1;

        // Exclusivo de WinForms: Ruta local del archivo para subirlo
        public string? RutaImagenLocal { get; set; }
    }
}