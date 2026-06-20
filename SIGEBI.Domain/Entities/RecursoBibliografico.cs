using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

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
            if (string.IsNullOrWhiteSpace(Titulo)) throw new BusinessExcepcion("El título es obligatorio.");
            Id = Guid.NewGuid();
            this.Titulo = Titulo;
            this.Autor = Autor;
            this.Categoria = Categoria;
            Estado = EstadoRecurso.Disponible;
        }

        public bool EspRestable()
        {
            return Estado == EstadoRecurso.Disponible;
        }

        public void MarcarComoPrestado()
        {
            if (Estado != EstadoRecurso.Disponible)
            {
                throw new BusinessExcepcion("El recurso no está disponible para préstamo.");
            }
            Estado = EstadoRecurso.Prestado;
        }

        public void MarcarComoDisponible()
        {
            Estado = EstadoRecurso.Disponible;
        }

       
    }
}