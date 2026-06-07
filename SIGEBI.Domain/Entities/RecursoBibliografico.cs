using SIGEBI.Domain.Enums;

namespace SIGEBI.Domain.Entities
{
    public class RecursoBibliografico
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public EstadoRecurso Estado { get; set; }

        public bool EsPrestable() => Estado == EstadoRecurso.Disponible;
        public void MarcarComoPrestado() => Estado = EstadoRecurso.Prestado;
        public void MarcarComoDisponible() => Estado = EstadoRecurso.Disponible;
    }
}