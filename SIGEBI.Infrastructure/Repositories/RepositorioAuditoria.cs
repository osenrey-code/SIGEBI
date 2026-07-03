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
                .FirstOrDefaultAsync(a => a.IdAuditoria == auditoriaId);
        }

        public async Task<IEnumerable<Auditoria>> ObtenerTodosAsync()
        {
            return await _dbSet
                .OrderByDescending(a => a.FechaRegistro)
                .ToListAsync();
        }

        public async Task AgregarAsync(Auditoria entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public Task ActualizarAsync(Auditoria entidad)
        {
            throw new NotSupportedException(
                "Los registros de auditoría no pueden ser modificados."
            );
        }

        public async Task<IEnumerable<Auditoria>> ObtenerPorEjecutorAsync(int usuarioId)
        {
            return await _dbSet
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auditoria>> ObtenerPorEntidadAsync(string entidad)
        {
            return await _dbSet
                .Where(a => a.EntidadAfectada.Contains(entidad.Trim()))
                .OrderByDescending(a => a.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auditoria>> ConsultarAsync(
            int? usuarioId,
            string? accion,
            string? entidadAfectada,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            var query = _dbSet.AsQueryable();

            if (usuarioId.HasValue)
                query = query.Where(a => a.UsuarioId == usuarioId.Value);

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
    }
}