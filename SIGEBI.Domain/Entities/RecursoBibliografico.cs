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

        public RecursoBibliografico(string isbn, string titulo, string autor, int categoriaId,
        int anioPublicado, string? imagenUrl)
        {
            //Validaciones de campos
            Guard.NotNullOrWhiteSpace(isbn, "El ISBN ");
            Guard.NotNullOrWhiteSpace(titulo, "El titulo del libro");
            Guard.NotNullOrWhiteSpace(imagenUrl, "La imagen del libro");
            Guard.NotNullOrWhiteSpace(autor, "El autor del libro.");

            ISBN = isbn.Trim();
            Titulo = titulo.Trim();
            Autor = autor.Trim();
            CategoriaId = categoriaId;
            ImagenUrl = imagenUrl?.Trim();
        }

        // Comportamientos 
        public bool TieneCopiasDisponible()
        {
            return CopiasDisponibles > 0;
        }

        public void AsignarImagen(string imagenUrl)
        {
            Guard.NotNullOrWhiteSpace(imagenUrl, "La ruta de la imagen");

            ImagenUrl = imagenUrl;
        }

        public void RegistrarNuevoEjemplar(string Identificador)
        {
            if (_ejemplares.Any(e => e.Identificador == Identificador))
            {
                throw new BusinessException($"Ya existe un ejemplar con el código {Identificador} en este recurso.");
            }

            var nuevoEjemplar = new Ejemplar(this.RecursoBibliograficoId, Identificador);

        }

    }
}