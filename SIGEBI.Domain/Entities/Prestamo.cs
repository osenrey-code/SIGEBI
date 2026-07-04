using SIGEBI.Domain.Common;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;

namespace SIGEBI.Domain.Entities
{
    public class Prestamo
    {
        public int PrestamoId { get; private set; }

        public int SolicitudId { get; private set;  }
        public virtual Solicitud? Solicitud { get; private set; }

        public int UsuarioId { get; private set; }
        public virtual Usuario? Usuario { get; private set;  }

        public int EjemplarId { get; private set; }
        public virtual Ejemplar? Ejemplar { get; private set; }

        public virtual Devolucion? Devolucion { get; private set;  }
        public DateTime FechaInicio { get; private set; }
        public DateTime FechaLimite { get; private set; }

        public EstadoPrestamo Estado { get; private set; }

        protected Prestamo() { }

        public Prestamo(int solicitudId, int usuarioId, int ejemplarId, int diasPermitidos)
        {
            if (solicitudId <= 0) throw new BusinessException("La solicitud de origen es inválida.");
            if (usuarioId <= 0) throw new BusinessException("El usuario es inválido.");
            if (ejemplarId <= 0) throw new BusinessException("El ejemplar es inválido.");
            if (diasPermitidos <= 0) throw new BusinessException("Los días permitidos deben ser mayores que cero.");

           SolicitudId = solicitudId;
           UsuarioId = usuarioId;
           EjemplarId = ejemplarId;

           FechaInicio = DateTime.UtcNow;
           FechaLimite = FechaInicio.AddDays(diasPermitidos);
           Estado = EstadoPrestamo.Activo;


        }

        public void MarcarComoDevuelto()
        {
            if (Estado != EstadoPrestamo.Activo && Estado != EstadoPrestamo.Vencido)
                throw new BusinessException("Solo se pueden devolver préstamos activos o vencidos.");
            Estado = EstadoPrestamo.Devuelto;
        }

        public bool EsDevolucionTardia(DateTime fechaEvaluacion)
        {
            return fechaEvaluacion.Date > FechaLimite.Date;
        }

        public int CalcularDiasRetraso(DateTime fechaEvaluacion)
        {
            if (!EsDevolucionTardia(fechaEvaluacion))
                return 0;

            return (fechaEvaluacion.Date - FechaLimite.Date).Days;
        }

        public void MarcarComoVencido()
        {
            if (Estado == EstadoPrestamo.Activo && EsDevolucionTardia(DateTime.UtcNow))
            {
                Estado = EstadoPrestamo.Vencido;
            }
        }
    }
}