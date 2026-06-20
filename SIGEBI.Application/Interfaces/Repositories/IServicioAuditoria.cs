using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IServicioAuditoria
    {
        Task RegistrarAccionAsync(string actorId, string tipoAccion, string modulo, string detalle);
        Task<IEnumerable<Auditoria>> ConsultarLogAsync(Guid? actorId, string tipoAccion, DateTime fechaInicio, DateTime FechaFin);
    }
}
