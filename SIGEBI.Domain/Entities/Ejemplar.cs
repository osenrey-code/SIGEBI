
using SIGEBI.Domain.Exceptions;
using SIGEBI.Domain.Common;
using SIGEBI.Domain.Enums;

namespace SIGEBI.Domain.Entities
{
    public class Ejemplar
    {
        public int EjemplarId { get; set; }

        public string Identificador { get; set; } = string.Empty;
        public int RecursoBibliograficoId { get; set; }
        public RecursoBibliografico RecursoBibliografico { get; private set; }
        public EstadoEjemplar Estado { get; set; }
        public string? Observacion { get; private set;}

        protected Ejemplar() { }

        public Ejemplar(int recursoBibliograficoId, string identificador)
        {
            Guard.NotNullOrWhiteSpace(Identificador, "El código identificador no puede estar vacío.");
            if (RecursoBibliograficoId <= 0)
                throw new BusinessException("El identificador del recurso bibliografico es inválido.");

            RecursoBibliograficoId = recursoBibliograficoId;
            Identificador = identificador;

            //Los ejemplares al agregarse estaran disponible
            Estado = EstadoEjemplar.Disponible;
        }

        public void MarcarComoPrestado()
        {
            if (Estado != EstadoEjemplar.Disponible)
                throw new BusinessException($"El ejmplar no se puede prestar. Estado actual: {Estado}");

            Estado = EstadoEjemplar.Prestado;
        }

        public void RegistrarDevolucion(string? observacion = null)
        {
            Guard.NotNullOrWhiteSpace(observacion, "Las observaciones de la entrega ");

            if (Estado != EstadoEjemplar.Prestado)
                throw new BusinessException("Solo se pueden devolver ejemplar en estado prestado.");

            Estado = EstadoEjemplar.Disponible;
            Observacion = observacion.Trim();
        }

        public void MarcarFueradeServicio(string motivo)
        {
            Guard.NotNullOrWhiteSpace(motivo, "El motivo ");

            if (Estado == EstadoEjemplar.Prestado)
                throw new BusinessException("No se puede marcar fuera de servicio un ejemplar que ha sido prestado.");

            Estado = EstadoEjemplar.FueraDeServicio;
            Observacion = motivo.Trim();
        }
    }
}
