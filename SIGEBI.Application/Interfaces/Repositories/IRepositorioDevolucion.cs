using SIGEBI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioDevolucion
    {
        Task AgregarAsync(Devolucion devolucion);
        Task<Devolucion?> ObtenerPorIdAsync(int devolucionId);
        Task<Devolucion?> ObtenerPorPrestamoIdAsync(int prestamoId);
        Task<IEnumerable<Devolucion>> ConsultarHistorialAsync(int? usuarioId, int? ejemplarId, DateTime? fechaInicio, DateTime? fechaFin);
    }
}
