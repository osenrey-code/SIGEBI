using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Response
{
    public class DevolucionResponse
    {
        public Guid PrestamoId { get; set; }
        public Guid PerfilLectorId { get; set; }
        public Guid RecursoId { get; set; }

        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaLimite { get; set; }
        public DateTime FechaDevolucion { get; set; }

        public string EstadoPrestamo { get; set; } = string.Empty;
        public bool FueTardia { get; set; }
        public int DiasRetraso { get; set; }
        public bool PenalizacionGenerada { get; set; }
    }
}
