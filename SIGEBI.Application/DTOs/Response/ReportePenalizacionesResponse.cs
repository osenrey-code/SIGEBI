using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Response
{
    public class ReportePenalizacionesResponse
    {
        public int TotalPenalizaciones { get; set; }
        public int PenalizacionesActivas { get; set; }
        public int PenalizacionesResueltas { get; set; }

        public int TotalDiasRetraso { get; set; }

        public decimal MontoTotalMora { get; set; }
        public decimal MontoMoraActiva { get; set; }
        public decimal MontoMoraResuelta { get; set; }
    }
}
