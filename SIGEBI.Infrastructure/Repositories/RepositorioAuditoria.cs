using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioAuditoria : IRepositorioAuditoria
    {
        private readonly SIGEBIDbContext _context;
        private readonly DbSet<Auditoria> _dbSet;

        public RepositorioAuditoria(SIGEBIDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Auditoria>();
        }

        public async Task<Auditoria?> ObtenerporIdAsync(object id)
        {
            var auditoriaId = Convert.ToInt32(id);

            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AuditoriaId == auditoriaId);
        }

        public async Task AgregarAsync(Auditoria entidad)
        {
            await _dbSet.AddAsync(entidad);
        }

        public async Task<IEnumerable<Auditoria>> ObtenerPorEjecutorAsync(int usuarioId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auditoria>> ObtenerPorEntidadAsync(string entidad)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(a => a.EntidadAfectada.Contains(entidad.Trim()))
                .OrderByDescending(a => a.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auditoria>> ConsultarAsync(
            string? identificacion,
            string? accion,
            string? entidadAfectada,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            var query = _dbSet
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(identificacion))
            {
                string filtroId = identificacion.Trim();

                var estudiantesIds = await _context.Set<Estudiante>()
                    .Where(e => e.Matricula.Contains(filtroId))
                    .Select(e => e.UsuarioId)
                    .ToListAsync();

                var docentesIds = await _context.Set<Docente>()
                    .Where(d => d.CodigoEmpleado.Contains(filtroId))
                    .Select(d => d.UsuarioId)
                    .ToListAsync();

                var adminsIds = await _context.Set<Administrador>()
                    .Where(a => a.CodigoEmpleado.Contains(filtroId))
                    .Select(a => a.UsuarioId)
                    .ToListAsync();

                var bibliotecariosIds = await _context.Set<Bibliotecario>()
                    .Where(b => b.CodigoEmpleado.Contains(filtroId))
                    .Select(b => b.UsuarioId)
                    .ToListAsync();

                var auditoresIds = await _context.Set<Auditor>()
                    .Where(au => au.CodigoEmpleado.Contains(filtroId))
                    .Select(au => au.UsuarioId)
                    .ToListAsync();

                var userIdsMatch = estudiantesIds
                    .Concat(docentesIds)
                    .Concat(adminsIds)
                    .Concat(bibliotecariosIds)
                    .Concat(auditoresIds)
                    .Distinct()
                    .ToList();

                query = query.Where(a => userIdsMatch.Contains(a.UsuarioId));
            }

            if (!string.IsNullOrWhiteSpace(accion))
                query = query.Where(a => a.Accion.Contains(accion.Trim()));

            if (!string.IsNullOrWhiteSpace(entidadAfectada))
                query = query.Where(a => a.EntidadAfectada.Contains(entidadAfectada.Trim()));

            if (fechaInicio.HasValue)
                query = query.Where(a => a.FechaRegistro >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(a => a.FechaRegistro <= fechaFin.Value);

            return await query
                .OrderByDescending(a => a.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auditoria>> ObtenerTodosAsync()
        {
            return await _dbSet
           .AsNoTracking()
           .OrderByDescending(a => a.FechaRegistro)
           .ToListAsync();
        }
    }
}