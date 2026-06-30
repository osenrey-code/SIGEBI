using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioPrestamo : IBaseRepository<Prestamo>
    {
       
        Task<IEnumerable<Prestamo>> ObtenerActivosPorPerfilLectorAsync(Guid perfilLectorId);
        Task<IEnumerable<Prestamo>> ObtenerHistorialPorPerfilLectorAsync(Guid perfilLectorId);
        Task<IEnumerable<Prestamo>> ObtenerPrestamosProximosAVencerAsync(DateTime fechaDesde, DateTime fechaHasta);
    }
}
