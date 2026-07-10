using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Common;
using SIGEBI.Application.DTOs.Response.ReporteResponse;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Enums;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioReporte : IRepositorioReporte
    {
        private readonly SIGEBIDbContext _context;

        public RepositorioReporte(SIGEBIDbContext context)
        {
            _context = context;
        }

        public async Task<ReporteInventarioResponse> ObtenerReporteInventarioAsync()
        {
            var recursos = await _context.RecursosBibliograficos
                .Include(r => r.Categoria)
                .Include(r => r.Ejemplares)
                .AsNoTracking()
                .ToListAsync();

            var detalles = recursos
                .Select(r => new DetalleInventarioReporteResponse
                {
                    RecursoBibliograficoId = r.RecursoBibliograficoId,
                    ISBN = r.ISBN,
                    Titulo = r.Titulo,
                    Categoria = r.Categoria != null
                        ? r.Categoria.Nombre
                        : "Sin categoría",

                    TotalEjemplares = r.Ejemplares.Count,

                    Disponibles = r.Ejemplares.Count(e =>
                        e.Estado == EstadoEjemplar.Disponible),

                    Prestados = r.Ejemplares.Count(e =>
                        e.Estado == EstadoEjemplar.Prestado),

                    Reservados = r.Ejemplares.Count(e =>
                        e.Estado == EstadoEjemplar.Reservado),

                    FueraDeServicio = r.Ejemplares.Count(e =>
                        e.Estado == EstadoEjemplar.FueraDeServicio)
                })
                .OrderBy(r => r.Categoria)
                .ThenBy(r => r.Titulo)
                .ToList();

            return new ReporteInventarioResponse
            {
                TotalRecursos = detalles.Count,
                TotalEjemplares = detalles.Sum(r => r.TotalEjemplares),
                EjemplaresDisponibles = detalles.Sum(r => r.Disponibles),
                EjemplaresPrestados = detalles.Sum(r => r.Prestados),
                EjemplaresReservados = detalles.Sum(r => r.Reservados),
                EjemplaresFueraDeServicio = detalles.Sum(r => r.FueraDeServicio),
                Recursos = detalles
            };
        }

        public async Task<ReporteUsoCatalogoResponse> ObtenerReporteUsoCatalogoAsync(
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            DateTime inicio = fechaInicio.Date;
            DateTime finExclusiva = fechaFin.Date.AddDays(1);

            var solicitudes = await _context.Solicitudes
                .Include(s => s.Ejemplar)
                    .ThenInclude(e => e!.RecursoBibliografico)
                        .ThenInclude(r => r!.Categoria)
                .AsNoTracking()
                .Where(s =>
                    s.FechaSolicitud >= inicio &&
                    s.FechaSolicitud < finExclusiva)
                .Select(s => new
                {
                    RecursoBibliograficoId =
                        s.Ejemplar != null &&
                        s.Ejemplar.RecursoBibliografico != null
                            ? s.Ejemplar.RecursoBibliografico.RecursoBibliograficoId
                            : 0,

                    Titulo =
                        s.Ejemplar != null &&
                        s.Ejemplar.RecursoBibliografico != null
                            ? s.Ejemplar.RecursoBibliografico.Titulo
                            : "Sin título",

                    Categoria =
                        s.Ejemplar != null &&
                        s.Ejemplar.RecursoBibliografico != null &&
                        s.Ejemplar.RecursoBibliografico.Categoria != null
                            ? s.Ejemplar.RecursoBibliografico.Categoria.Nombre
                            : "Sin categoría"
                })
                .ToListAsync();

            var recursos = await _context.RecursosBibliograficos
                .Include(r => r.Ejemplares)
                .AsNoTracking()
                .ToListAsync();

            int totalEjemplares = recursos.Sum(r => r.Ejemplares.Count);

            int ejemplaresDisponibles = recursos.Sum(r =>
                r.Ejemplares.Count(e => e.Estado == EstadoEjemplar.Disponible));

            decimal disponibilidadPromedio = totalEjemplares > 0
                ? Math.Round(
                    (decimal)ejemplaresDisponibles / totalEjemplares * 100,
                    2)
                : 0;

            var recursosMasSolicitados = solicitudes
                .Where(s => s.RecursoBibliograficoId > 0)
                .GroupBy(s => new
                {
                    s.RecursoBibliograficoId,
                    s.Titulo
                })
                .Select(grupo => new RecursoMasSolicitadoResponse
                {
                    RecursoBibliograficoId = grupo.Key.RecursoBibliograficoId,
                    Titulo = grupo.Key.Titulo,
                    CantidadSolicitudes = grupo.Count()
                })
                .OrderByDescending(r => r.CantidadSolicitudes)
                .ThenBy(r => r.Titulo)
                .ToList();

            var demandaPorCategoria = solicitudes
                .GroupBy(s => s.Categoria)
                .Select(grupo => new DemandaCategoriaResponse
                {
                    Categoria = grupo.Key,
                    CantidadSolicitada = grupo.Count()
                })
                .OrderByDescending(c => c.CantidadSolicitada)
                .ThenBy(c => c.Categoria)
                .ToList();

            return new ReporteUsoCatalogoResponse
            {
                TotalSolicitudes = solicitudes.Count,
                DisponibilidadPromedio = disponibilidadPromedio,
                RecursosMasSolicitados = recursosMasSolicitados,
                DemandaPorCategoria = demandaPorCategoria
            };
        }

        public async Task<ReportePrestamoResponse> ObtenerReportePrestamoAsync(
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            DateTime inicio = fechaInicio.Date;
            DateTime finExclusiva = fechaFin.Date.AddDays(1);

            var prestamos = await _context.Prestamos
                .Include(p => p.Ejemplar)
                    .ThenInclude(e => e!.RecursoBibliografico)
                .Include(p => p.Devolucion)
                .AsNoTracking()
                .Where(p =>
                    p.FechaInicio >= inicio &&
                    p.FechaInicio < finExclusiva)
                .ToListAsync();

            int totalPrestamos = prestamos.Count;

            int devueltosATiempo = prestamos.Count(p =>
                p.Devolucion != null &&
                p.Devolucion.FechaDevolucion <= p.FechaLimite);

            int vencidos = prestamos.Count(p =>
                (
                    p.Devolucion != null &&
                    p.Devolucion.FechaDevolucion > p.FechaLimite
                )
                ||
                (
                    p.Devolucion == null &&
                    DateTime.UtcNow > p.FechaLimite
                ));

            decimal tasaPuntual = totalPrestamos > 0
                ? Math.Round((decimal)devueltosATiempo / totalPrestamos * 100, 2)
                : 0;

            var detalles = prestamos
                .Select(p => new DetallePrestamoReporteResponse
                {
                    PrestamoId = p.PrestamoId,

                    RecursoBibliograficoId =
                        p.Ejemplar?.RecursoBibliografico?.RecursoBibliograficoId ?? 0,

                    TituloRecurso =
                        p.Ejemplar?.RecursoBibliografico?.Titulo ?? "Sin título",

                    IdentificadorEjemplar =
                        p.Ejemplar?.Identificador ?? "Sin identificador",

                    FechaPrestamo = p.FechaInicio,

                    FechaLimite = p.FechaLimite,

                    FechaDevolucion = p.Devolucion?.FechaDevolucion,

                    Estado = p.Estado.ToString()
                })
                .OrderByDescending(p => p.FechaPrestamo)
                .ToList();

            return new ReportePrestamoResponse
            {
                TotalPrestamos = totalPrestamos,
                PrestamosDevueltosATiempo = devueltosATiempo,
                PrestamosVencidos = vencidos,
                TasaDevolucionPuntual = tasaPuntual,
                Prestamos = detalles
            };
        }

        public async Task<ReportePenalizacionesResponse> ObtenerReportePenalizacionesAsync(
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            DateTime inicio = fechaInicio.Date;
            DateTime finExclusiva = fechaFin.Date.AddDays(1);

            var penalizaciones = await _context.Penalizaciones
                .Include(p => p.Usuario)
                .AsNoTracking()
                .Where(p =>
                    p.FechaGeneracion >= inicio &&
                    p.FechaGeneracion < finExclusiva)
                .ToListAsync();

            int total = penalizaciones.Count;

            int activas = penalizaciones.Count(p =>
                p.Estado == EstadoPenalizacion.Activa);

            int resueltas = penalizaciones.Count(p =>
                p.Estado == EstadoPenalizacion.Resuelta);

            int totalDiasRetraso = penalizaciones.Sum(p =>
                p.DiasRetraso);

            decimal montoTotal = penalizaciones.Sum(p =>
                p.MontoMora);

            decimal montoActivo = penalizaciones
                .Where(p => p.Estado == EstadoPenalizacion.Activa)
                .Sum(p => p.MontoMora);

            decimal montoResuelto = penalizaciones
                .Where(p => p.Estado == EstadoPenalizacion.Resuelta)
                .Sum(p => p.MontoMora);

            var porTipoUsuario = penalizaciones
                .GroupBy(p => p.Usuario.ObtenerTipoUsuario())
                .Select(grupo => new PenalizacionPorTipoUsuarioResponse
                {
                    TipoUsuario = grupo.Key,

                    Generadas = grupo.Count(),

                    Activas = grupo.Count(p =>
                        p.Estado == EstadoPenalizacion.Activa),

                    Resueltas = grupo.Count(p =>
                        p.Estado == EstadoPenalizacion.Resuelta),

                    MontoTotal = grupo.Sum(p =>
                        p.MontoMora)
                })
                .OrderBy(p => p.TipoUsuario)
                .ToList();

            var detalles = penalizaciones
                .Select(p => new DetallePenalizacionReporteResponse
                {
                    PenalizacionId = p.PenalizacionId,

                    UsuarioId = p.UsuarioId,

                    TipoUsuario = p.Usuario.ObtenerTipoUsuario(),

                    Motivo = p.Motivo,

                    DiasRetraso = p.DiasRetraso,

                    MontoMora = p.MontoMora,

                    FechaGeneracion = p.FechaGeneracion,

                    Estado = p.Estado.ToString()
                })
                .OrderByDescending(p => p.FechaGeneracion)
                .ToList();

            return new ReportePenalizacionesResponse
            {
                TotalPenalizaciones = total,
                PenalizacionesActivas = activas,
                PenalizacionesResueltas = resueltas,
                TotalDiasRetraso = totalDiasRetraso,
                MontoTotalMora = montoTotal,
                MontoMoraActiva = montoActivo,
                MontoMoraResuelta = montoResuelto,
                PorTipoUsuario = porTipoUsuario,
                Detalles = detalles
            };
        }
    }
}