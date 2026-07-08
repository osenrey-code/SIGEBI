using SIGEBI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface ISolicitudRepository
    {
        Task AgregarAsync(Solicitud solicitud);
        Task<Solicitud?> ObtenerPorIdAsync(int id);
        Task<Solicitud?> ObtenerConDetallesAsync(int id);
        Task<IEnumerable<Solicitud>> ObtenerTodasAsync();
        Task<IEnumerable<Solicitud>> ObtenerPendientesAsync();
        Task ActualizarAsync(Solicitud solicitud);
    }
}
