using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioPrestamo 
    {
        Task AgregarAsync(Prestamo prestamo);
        Task<int> ContarActivosPorUsuarioAsync(int usuarioId);
        Task<Prestamo?> ObtenerPorIdAsync(int id);
        Task ActualizarAsync(Prestamo prestamo);
        Task<Prestamo?> ObtenerConDetallesAsync(int id);
        Task<IEnumerable<Prestamo>> ObtenerTodosAsync();
        Task<IEnumerable<Prestamo>> ObtenerActivosPorUsuarioAsync(int usuarioId);
        Task<IEnumerable<Prestamo>> ObtenerActivosVencidosAsync(DateTime fechaEvaluacion);
    }
}
