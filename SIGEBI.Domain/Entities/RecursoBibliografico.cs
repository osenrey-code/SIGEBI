using SIGEBI.Domain.Common;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
{
    public class RecursoBibliografico
    {
        public int  RecursoBibliograficoId { get; set; }
        public string ISBN { get; private set; } = string.Empty;
        public string Titulo { get; private set; } = string.Empty;
        public string Autor { get;  private set; } = string.Empty;
        public int AnioPublicado { get; private set; }
        public string? ImagenUrl { get;  private set; }
        public int CategoriaId { get; private set; }
        public virtual Categoria? Categoria { get; private set; }

        //Coleccion privada para que se pueda agregar libros desde fuera
        private readonly List<Ejemplar> _ejemplares = new();

        //Propiedad para exponer los ejemplares
        public IReadOnlyCollection<Ejemplar> Ejemplares => _ejemplares.AsReadOnly();

        //Total de libros registrados
        public int TotalEjemplares => _ejemplares.Count;

        public int CopiasDisponibles => _ejemplares.Count(e => e.Estado == EstadoEjemplar.Disponible);
        protected RecursoBibliografico() { }

        

    }
}