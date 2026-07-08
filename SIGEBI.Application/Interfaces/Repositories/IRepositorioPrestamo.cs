using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioPrestamo 
    {
        Task AgregarAsync(Prestamo prestamo);
        Task ActualizarAsync(Prestamo prestamo);

        Task<int> ContarActivosPorUsuarioAsync(int usuarioId);
        Task<IEnumerable<Prestamo>> ObtenerActivosVencidosAsync(DateTime fechaEvaluacion);

        Task<Prestamo?> ObtenerPorIdAsync(int id);
        Task<Prestamo?> ObtenerConDetallesAsync(int id);

        Task<IEnumerable<Prestamo>> ConsultarActivosAsync(int? usuarioId, int? ejemplarId);
        Task<IEnumerable<Prestamo>> ConsultarHistorialAsync(int? usuarioId, int? ejemplarId);
    }
}

