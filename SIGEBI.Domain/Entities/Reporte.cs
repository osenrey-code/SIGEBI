using SIGEBI.Domain.Common;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Domain.Entities
{
    public class Reporte
    {
        public int IdReporte { get; private set; }

        public string Tipo { get; private set;  } = string.Empty;
        public DateTime FechaSolicitud { get; private set;  }
        public DateTime? FechaGenerado { get; private set; }// Nullable es porque aun no se ha generado
        public string IdUsuario { get; private set; } = string.Empty;
        public EstadoReporte Estado { get; private set; }
        public string ArchivoGenerado { get; private set; } = string.Empty;// Ubicacion del archivo generado

        protected Reporte() { }

        public Reporte(string Tipo, string IdUsuario)
        {
            Guard.NotNullOrWhiteSpace(Tipo, "El tipo ");
            Guard.NotNullOrWhiteSpace(IdUsuario, "El usuario ");

            this.Tipo = Tipo;
            this.IdUsuario = IdUsuario;
            FechaSolicitud = DateTime.Now;
            Estado = EstadoReporte.Pendiente;
            ArchivoGenerado = string.Empty;
        }

        public void AgregarArchivoYFinalizar(string archivo) {
            Guard.NotNullOrWhiteSpace(archivo, "El archivo ");

            if (Estado != EstadoReporte.Pendiente)
                throw new BusinessException($"Para completar el reporte no puede estar en estado '{Estado}'.");
            ArchivoGenerado = archivo;
            Estado = EstadoReporte.Finalizado;
            FechaGenerado = DateTime.Now;
        }

        public void Cancelar()
        {
            if (Estado != EstadoReporte.Pendiente)
                throw new BusinessException($"No se puede cancelar un reporte que esta en estado '{Estado}'.");
            Estado = EstadoReporte.Fallido;
            FechaGenerado = DateTime.Now;
        }
    }
}
