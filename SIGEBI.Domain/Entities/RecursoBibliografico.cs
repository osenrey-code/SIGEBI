using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
{
    public class RecursoBibliografico
    {
        public Guid Id { get; private set; }
        public string Identificador { get; private set; } = string.Empty;
        public int NumeroEjemplares { get; private set; }
        public string Titulo { get; private set; } = string.Empty;
        public string Autor { get; private set; } = string.Empty;
        public string Categoria { get; private set; } = string.Empty;
        public EstadoRecurso Estado { get; private set; }

        private RecursoBibliografico() { }

        public RecursoBibliografico(
        string identificador,
        string titulo,
        string autor,
        string categoria,
        int numeroEjemplares)
        {
            if (string.IsNullOrWhiteSpace(identificador))
                throw new BusinessException("El identificador del recurso es obligatorio.");

            if (string.IsNullOrWhiteSpace(titulo))
                throw new BusinessException("El título es obligatorio.");

            if (string.IsNullOrWhiteSpace(autor))
                throw new BusinessException("El autor es obligatorio.");

            if (string.IsNullOrWhiteSpace(categoria))
                throw new BusinessException("La categoría es obligatoria.");

            if (numeroEjemplares <= 0)
                throw new BusinessException("El número de ejemplares debe ser mayor que cero.");

            Id = Guid.NewGuid();
            Identificador = identificador.Trim();
            Titulo = titulo.Trim();
            Autor = autor.Trim();
            Categoria = categoria.Trim();
            NumeroEjemplares = numeroEjemplares;
            Estado = EstadoRecurso.Disponible;
        }

        public bool EstaDisponible()
        {
            return Estado == EstadoRecurso.Disponible;
        }

        public void ActualizarInformacion(
        string identificador,
        string titulo,
        string autor,
        string categoria,
        int numeroEjemplares)
        {
            if (string.IsNullOrWhiteSpace(identificador))
                throw new BusinessException("El identificador del recurso es obligatorio.");

            if (string.IsNullOrWhiteSpace(titulo))
                throw new BusinessException("El título es obligatorio.");

            if (string.IsNullOrWhiteSpace(autor))
                throw new BusinessException("El autor es obligatorio.");

            if (string.IsNullOrWhiteSpace(categoria))
                throw new BusinessException("La categoría es obligatoria.");

            if (numeroEjemplares <= 0)
                throw new BusinessException("El número de ejemplares debe ser mayor que cero.");

            Identificador = identificador.Trim();
            Titulo = titulo.Trim();
            Autor = autor.Trim();
            Categoria = categoria.Trim();
            NumeroEjemplares = numeroEjemplares;
        }

        public void MarcarComoPrestado()
        {
            if (Estado != EstadoRecurso.Disponible)
            {
                throw new BusinessException("El recurso no está disponible para préstamo.");
            }
            Estado = EstadoRecurso.Prestado;
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

        public void MarcarComoReservado()
        {
            if (Estado != EstadoRecurso.Disponible)
            {
                throw new BusinessException("Solo se puede reservar un recurso disponible.");
            }

            Estado = EstadoRecurso.Reservado;
        }

        public void MarcarFueraDeServicio()
        {
            if (Estado == EstadoRecurso.Prestado)
            {
                throw new BusinessException("No se puede poner fuera de servicio un recurso prestado.");
            }

            Estado = EstadoRecurso.FueraDeServicio;
        }


    }
}