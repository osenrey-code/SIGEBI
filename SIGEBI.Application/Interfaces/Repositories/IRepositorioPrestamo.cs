using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioPrestamo : IBaseRepository<Prestamo>
    {
       
        Task<IEnumerable<Prestamo>> ObtenerActivosPorUsuarioAsync(Guid usuarioId);
        Task<IEnumerable<Prestamo>> ObtenerHistorialPorUsuarioAsync(Guid usuarioId);
        Task<IEnumerable<Prestamo>> ObtenerPrestamosProximosAVencerAsync(
    DateTime fechaDesde,
    DateTime fechaHasta);
    }
}
