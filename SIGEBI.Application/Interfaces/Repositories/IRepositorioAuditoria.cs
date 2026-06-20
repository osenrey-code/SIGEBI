using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioAuditoria : ReadOnly<Auditoria>, Writer<Auditoria>
    {
        Task<IEnumerable<Auditoria>> ObtenerPorUsuarioAsync(Guid usuarioId);

        Task<IEnumerable<Auditoria>> ObtenerPorRangoFechaAsync(DateTime fechaInicio, DateTime fechaFin);

        Task RegistrarAccionAsync(string usuarioId, string tipoAccion, string modulo, string detalle);

        Task<IEnumerable<Auditoria>> ConsultarLogAsync(Guid? usuarioId, string? tipoAccion, DateTime? fechaInicio, DateTime? fechaFin);
    }
}
