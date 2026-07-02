using SIGEBI.Domain.Common;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
{
    public class RecursoBibliografico
    {
        public string ISBN { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get;  set; } = string.Empty;
        public int NumeroEjemplares { get;  set; }
        public int Copias { get; set; }
        public int AnioPublicado { get; set; }
        public string? ImagenUrl { get;  set; }
        public EstadoRecurso Estado { get;  set; }
        public int CategoriaId { get; set; }
        public virtual Categoria? Categoria { get; set; }

        protected RecursoBibliografico() { }

        public RecursoBibliografico(string isbn, string titulo, string autor, int categoria,
        int numeroEjemplares, string imagenUrl)
        {
            //Validaciones de campos
            Guard.NotNullOrWhiteSpace(isbn, "El ISBN ");
            Guard.NotNullOrWhiteSpace(titulo, "El titulo del libro");
            Guard.NotNullOrWhiteSpace(imagenUrl, "La imagen del libro");
            Guard.NotNullOrWhiteSpace(autor, "El autor del libro.");

            ISBN = isbn.Trim();
            Titulo = titulo.Trim();
            Autor = autor.Trim();
            CategoriaId = categoria;
            Estado = EstadoRecurso.Disponible;
        }

        public bool EstaDisponible()
        {
            return Estado == EstadoRecurso.Disponible;
        }

        public void AsignarImagen(string imagenUrl)
        {
            Guard.NotNullOrWhiteSpace(imagenUrl, "La ruta de la imagen");

            ImagenUrl = imagenUrl;
        }

        public void MarcarComoDisponible()
        {
            Estado = EstadoRecurso.Disponible;
        }

        public void CambiarEstado(EstadoRecurso nuevoEstado)
        {
            if (Estado == EstadoRecurso.Prestado && nuevoEstado == EstadoRecurso.FueraDeServicio)
            {
                throw new BusinessException("No se puede poner fuera de servicio un recurso prestado.");
            }

            Estado = nuevoEstado;
        }

        public void MarcarFueraDeServicio()
        {
            if (Estado == EstadoRecurso.Prestado)
            {
                throw new BusinessException("No se puede poner fuera de servicio un recurso prestado.");
            }

            Estado = EstadoRecurso.FueraDeServicio;
        } 

        public void Incrementar()
        {
            Copias++;
            NumeroEjemplares++;
        }

        public void Devolver()
        {
            if (Copias >= NumeroEjemplares)
            {
                throw new BusinessException("No se pueden devolver más copias de las totales registradas.");
            }
            Copias++;
            
        }

    }
}