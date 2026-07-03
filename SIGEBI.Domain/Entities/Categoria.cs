using SIGEBI.Domain.Common;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
{
    public class Categoria
    {
        public int CategoriaId { get; private set; }

        public string Nombre { get; private set; } = string.Empty;

        public string Descripcion { get; private set; } = string.Empty;

        public virtual ICollection<RecursoBibliografico> Libros { get; private set; } =
            new List<RecursoBibliografico>();

        protected Categoria() { }

        public Categoria(string nombre, string descripcion)
        {
            Guard.NotNullOrWhiteSpace(nombre, "El nombre de la categoría");

            Nombre = nombre.Trim();
            Descripcion = descripcion?.Trim() ?? string.Empty;
        }

        public void Actualizar(string nombre, string descripcion)
        {
            Guard.NotNullOrWhiteSpace(nombre, "El nombre de la categoría");

            Nombre = nombre.Trim();
            Descripcion = descripcion?.Trim() ?? string.Empty;
        }
    }
}