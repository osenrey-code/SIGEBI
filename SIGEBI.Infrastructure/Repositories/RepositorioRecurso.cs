using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioRecurso : RepositorioBase<RecursoBibliografico>, IRepositorioRecurso
    {
        public RepositorioRecurso(SIGEBIDbContext context) : base(context)
        {
        }

        public async Task<RecursoBibliografico?> BuscarPorIsbnAsync(string isbn)
        {
            return await _dbSet
                .Include(r => r.Categoria)
                .Include(r => r.Ejemplares)
                .FirstOrDefaultAsync(r => r.ISBN.ToLower() == isbn.Trim().ToLower());
        }

        public async Task<RecursoBibliografico?> BuscarConCategoriaAsync(int recursoBibliograficoId)
        {
            return await _dbSet
                .Include(r => r.Categoria)
                .Include(r => r.Ejemplares)
                .FirstOrDefaultAsync(r => r.RecursoBibliograficoId == recursoBibliograficoId);
        }

        public async Task<IEnumerable<RecursoBibliografico>> ConsultarCatalogoAsync(
            string? titulo,
            string? autor,
            string? categoria,
            bool? soloDisponibles)
        {
            var query = _dbSet
                .Include(r => r.Categoria)
                .Include(r => r.Ejemplares)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(titulo))
            {
                query = query.Where(r => r.Titulo.Contains(titulo.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(autor))
            {
                query = query.Where(r => r.Autor.Contains(autor.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(categoria))
            {
                query = query.Where(r =>
                    r.Categoria != null &&
                    r.Categoria.Nombre.Contains(categoria.Trim()));
            }

            if (soloDisponibles == true)
            {
                query = query.Where(r =>
                    r.Ejemplares.Any(e => e.Estado == EstadoEjemplar.Disponible));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ReporteUsoCatalogoResponse>> ObtenerEstadisticasUsoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var estadisticasInventario = await _dbSet
         .Include(r => r.Categoria)
         .Include(r => r.Ejemplares)
         .AsNoTracking()
         .GroupBy(r => r.Categoria != null ? r.Categoria.Nombre : "Sin categoría")
         .Select(grupo => new
         {
             Categoria = grupo.Key,
             TotalEjemplares = grupo.SelectMany(r => r.Ejemplares).Count(),
             EjemplaresDisponibles = grupo
                 .SelectMany(r => r.Ejemplares)
                 .Count(e => e.Estado == EstadoEjemplar.Disponible)
         })
         .ToListAsync();

            var estadisticasPrestamos = await _context.Prestamos
                .Include(p => p.Ejemplar)
                    .ThenInclude(e => e!.RecursoBibliografico)
                        .ThenInclude(r => r!.Categoria)
                .AsNoTracking()
                .Where(p => p.FechaInicio >= fechaInicio &&
                            p.FechaInicio <= fechaFin)
                .Select(p => new
                {
                    Categoria = p.Ejemplar != null &&
                                p.Ejemplar.RecursoBibliografico != null &&
                                p.Ejemplar.RecursoBibliografico.Categoria != null
                        ? p.Ejemplar.RecursoBibliografico.Categoria.Nombre
                        : "Sin categoría",

                    Titulo = p.Ejemplar != null &&
                             p.Ejemplar.RecursoBibliografico != null
                        ? p.Ejemplar.RecursoBibliografico.Titulo
                        : "Sin título"
                })
                .GroupBy(x => x.Categoria)
                .Select(grupo => new
                {
                    Categoria = grupo.Key,
                    TotalPrestamos = grupo.Count(),
                    RecursoMasSolicitado = grupo
                        .GroupBy(x => x.Titulo)
                        .OrderByDescending(t => t.Count())
                        .Select(t => t.Key)
                        .FirstOrDefault()
                })
                .ToListAsync();

            var reporte = estadisticasInventario
                .Select(inventario =>
                {
                    var prestamosCategoria = estadisticasPrestamos
                        .FirstOrDefault(p => p.Categoria == inventario.Categoria);

                    return new ReporteUsoCatalogoResponse
                    {
                        Categoria = inventario.Categoria,
                        TotalPrestamos = prestamosCategoria?.TotalPrestamos ?? 0,
                        RecursoMasSolicitado = prestamosCategoria?.RecursoMasSolicitado ?? "Ninguno",

                        DisponibilidadPromedio = inventario.TotalEjemplares > 0
                            ? Math.Round(
                                (decimal)inventario.EjemplaresDisponibles /
                                inventario.TotalEjemplares * 100,
                                2)
                            : 0
                    };
                })
                .OrderByDescending(r => r.TotalPrestamos)
                .ThenBy(r => r.Categoria)
                .ToList();

            return reporte;
        }

        public async Task<IEnumerable<ReporteInventarioResponse>> ObtenerReporteInventarioAsync()
        {
            return await _dbSet
        .Include(r => r.Categoria)
        .Include(r => r.Ejemplares)
        .AsNoTracking()
        .Select(r => new ReporteInventarioResponse
        {
            RecursoBibliograficoId = r.RecursoBibliograficoId,
            ISBN = r.ISBN,
            Titulo = r.Titulo,
            Autor = r.Autor,
            Categoria = r.Categoria != null ? r.Categoria.Nombre : "Sin categoría",

            TotalEjemplares = r.Ejemplares.Count(),
            Disponibles = r.Ejemplares.Count(e => e.Estado == EstadoEjemplar.Disponible),
            Prestados = r.Ejemplares.Count(e => e.Estado == EstadoEjemplar.Prestado),
            Reservados = r.Ejemplares.Count(e => e.Estado == EstadoEjemplar.Reservado),
            FueraDeServicio = r.Ejemplares.Count(e => e.Estado == EstadoEjemplar.FueraDeServicio)
        })
        .OrderBy(r => r.Categoria)
        .ThenBy(r => r.Titulo)
        .ToListAsync();
        }
    }
}