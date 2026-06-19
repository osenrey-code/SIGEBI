using SIGEBI.Domain.Enums;

namespace SIGEBI.Domain.Entities
{
    public class RecursoBibliografico
    {
        public Guid Id { get; private set; }
        public string Titulo { get; private set; } 
        public string Autor { get; private set; } 
        public string Categoria { get; private set; }
        public EstadoRecurso Estado { get; private set; }

        private RecursoBibliografico() { }

        public RecursoBibliografico(string Titulo, string Autor, string Categoria)
        {
            Id = Guid.NewGuid();
            this.Titulo = Titulo;
            this.Autor = Autor;
            this.Categoria = Categoria;
            Estado = EstadoRecurso.Disponible;
        }

        public void MarcarComoPrestado()
        {
            if (Estado != EstadoRecurso.Disponible)
            {
                throw new Exception("El recurso no está disponible para préstamo.");
            }
            Estado = EstadoRecurso.Prestado;
        }

        public void MarcarComoDisponible()
        {
            Estado = EstadoRecurso.Disponible;
        }

       
    }
}