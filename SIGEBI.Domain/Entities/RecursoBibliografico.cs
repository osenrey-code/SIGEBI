using SIGEBI.Domain.Common;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
{
    public class RecursoBibliografico
    {
        public int RecursoBibliograficoId { get; private set; }

        public string ISBN { get; private set; } = string.Empty;

        public string Titulo { get; private set; } = string.Empty;

        public string Autor { get; private set; } = string.Empty;

        public int AnioPublicado { get; private set; }

        public string? ImagenUrl { get; private set; }

        public int CategoriaId { get; private set; }

        public virtual Categoria? Categoria { get; private set; }

        private readonly List<Ejemplar> _ejemplares = new();

        public IReadOnlyCollection<Ejemplar> Ejemplares => _ejemplares.AsReadOnly();

        public int TotalEjemplares { get; private set; }

        public int CopiasDisponibles { get; private set; }
            

        protected RecursoBibliografico() { }

        public RecursoBibliografico(
            string isbn,
            string titulo,
            string autor,
            int categoriaId,
            int anioPublicado,
            string? imagenUrl)
        {
            Guard.NotNullOrWhiteSpace(isbn, "El ISBN");
            Guard.NotNullOrWhiteSpace(titulo, "El título del libro");
            Guard.NotNullOrWhiteSpace(autor, "El autor del libro");

            if (categoriaId <= 0)
                throw new BusinessException("La categoría del recurso es inválida.");

            if (anioPublicado <= 0)
                throw new BusinessException("El año de publicación es inválido.");

            ISBN = isbn.Trim();
            Titulo = titulo.Trim();
            Autor = autor.Trim();
            CategoriaId = categoriaId;
            AnioPublicado = anioPublicado;
            ImagenUrl = imagenUrl?.Trim();
        }

        public bool TieneCopiasDisponibles()
        {
            return CopiasDisponibles > 0;
        }

        public void ActualizarInformacion(
            string titulo,
            string autor,
            int categoriaId,
            int anioPublicado,
            string? imagenUrl)
        {
            Guard.NotNullOrWhiteSpace(titulo, "El título del libro");
            Guard.NotNullOrWhiteSpace(autor, "El autor del libro");

            if (categoriaId <= 0)
                throw new BusinessException("La categoría del recurso es inválida.");

            if (anioPublicado <= 0)
                throw new BusinessException("El año de publicación es inválido.");

            Titulo = titulo.Trim();
            Autor = autor.Trim();
            CategoriaId = categoriaId;
            AnioPublicado = anioPublicado;
            ImagenUrl = imagenUrl?.Trim();
        }

        public void AsignarImagen(string imagenUrl)
        {
            Guard.NotNullOrWhiteSpace(imagenUrl, "La ruta de la imagen");

            ImagenUrl = imagenUrl.Trim();
        }

        public void RegistrarNuevoEjemplar(string identificador)
        {
            Guard.NotNullOrWhiteSpace(identificador, "El identificador del ejemplar");

            if (_ejemplares.Any(e => e.Identificador == identificador.Trim()))
            {
                throw new BusinessException(
                    $"Ya existe un ejemplar con el código {identificador} en este recurso."
                );
            }

            var nuevoEjemplar = new Ejemplar(RecursoBibliograficoId, identificador.Trim());

            _ejemplares.Add(nuevoEjemplar);

            TotalEjemplares++;
            CopiasDisponibles++;
        }
    }
}