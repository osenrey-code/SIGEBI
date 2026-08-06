using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Application.DTOs.Response;
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

        Task<IEnumerable<Prestamo>> ConsultarActivosAsync(
             string? Identificacion,
             int? recursoBibliograficoId,
             int? ejemplarId
        );
        Task<IEnumerable<Prestamo>> ConsultarHistorialAsync(
            string? identificacion,
            int? recursoBibliograficoId,
            int? ejemplarId
        );
        Task<bool> ExistePrestamoActivoPorRecursoAsync(int recursoBibliograficoId);
        Task<bool> TienePrestamoActivoDeRecursoAsync(int usuarioId, int recursoBibliograficoId);


    }
}

