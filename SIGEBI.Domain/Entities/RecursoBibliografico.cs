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

        public ICollection<Ejemplar> Ejemplares { get; private set; } = new List<Ejemplar>();

        public int TotalEjemplares => Ejemplares.Count;

        public int CopiasDisponibles => Ejemplares.Count(e => e.Estado == EstadoEjemplar.Disponible);

        public bool Activo { get; private set; } = true;



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

        public void Desactivar()
        {
            Activo = false;
        }

        public void Activar()
        {
            Activo = true;
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

            if (Ejemplares.Any(e => e.Identificador == identificador.Trim()))
            {
                throw new BusinessException(
                    $"Ya existe un ejemplar con el código {identificador} en este recurso."
                );
            }

            var nuevoEjemplar = new Ejemplar(RecursoBibliograficoId, identificador.Trim());

            Ejemplares.Add(nuevoEjemplar);
            
        }

        public void AgregarEjemplares(int cantidad)
        {
            Guard.GreaterThan(cantidad, 0, "la cantidad de ejemplares ");

            int siguiente = Ejemplares.Count + 1;

            for (int i = 0; i < cantidad; i++)
            {
                int secuencia = siguiente + i;
                string identificador = $"{ISBN}-{secuencia:D3}";

                while (Ejemplares.Any(e => e.Identificador == identificador))
                {
                    secuencia++;
                    identificador = $"{ISBN}-{secuencia:D3}";
                }

                var nuevoEjemplar = new Ejemplar(RecursoBibliograficoId, identificador);
                Ejemplares.Add(nuevoEjemplar);
            }
        }
    }
}