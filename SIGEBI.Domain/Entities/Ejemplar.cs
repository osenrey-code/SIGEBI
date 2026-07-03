using SIGEBI.Domain.Common;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
{
    public class Ejemplar
    {
        public int EjemplarId { get; private set; }

        public string Identificador { get; private set; } = string.Empty;

        public int RecursoBibliograficoId { get; private set; }

        public RecursoBibliografico? RecursoBibliografico { get; private set; }

        public EstadoEjemplar Estado { get; private set; }

        public string? Observacion { get; private set; }

        protected Ejemplar() { }

        public Ejemplar(int recursoBibliograficoId, string identificador)
        {
            Guard.NotNullOrWhiteSpace(identificador, "El código identificador del ejemplar");

            if (recursoBibliograficoId < 0)
                throw new BusinessException("El identificador del recurso bibliográfico es inválido.");

            RecursoBibliograficoId = recursoBibliograficoId;
            Identificador = identificador.Trim();
            Estado = EstadoEjemplar.Disponible;
        }

        public void MarcarComoPrestado()
        {
            if (Estado != EstadoEjemplar.Disponible)
                throw new BusinessException($"El ejemplar no se puede prestar. Estado actual: {Estado}");

            Estado = EstadoEjemplar.Prestado;
            Observacion = null;
        }

        public void MarcarComoReservado()
        {
            if (Estado != EstadoEjemplar.Disponible)
                throw new BusinessException($"El ejemplar no se puede reservar. Estado actual: {Estado}");

            Estado = EstadoEjemplar.Reservado;
            Observacion = null;
        }

        public void RegistrarDevolucion(string? observacion = null)
        {
            if (Estado != EstadoEjemplar.Prestado)
                throw new BusinessException("Solo se pueden devolver ejemplares en estado prestado.");

            Estado = EstadoEjemplar.Disponible;
            Observacion = observacion?.Trim();
        }

        public void MarcarFueraDeServicio(string motivo)
        {
            Guard.NotNullOrWhiteSpace(motivo, "El motivo");

            if (Estado == EstadoEjemplar.Prestado)
                throw new BusinessException("No se puede marcar fuera de servicio un ejemplar que está prestado.");

            Estado = EstadoEjemplar.FueraDeServicio;
            Observacion = motivo.Trim();
        }

        public void MarcarDisponible()
        {
            Estado = EstadoEjemplar.Disponible;
            Observacion = null;
        }
    }
}