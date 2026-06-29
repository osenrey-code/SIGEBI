using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioAuditoria : IRepositorioAuditoria
    {
        private readonly SIGEBIDbContext _context;
        private readonly DbSet<RegistroAuditoria> _dbSet;

        public RepositorioAuditoria(SIGEBIDbContext context)
        {
            _context = context;
            _dbSet = context.Set<RegistroAuditoria>();
        }

        public async Task<RegistroAuditoria?> ObtenerPorIdAsync(object id)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.Id == (Guid)id);
        }

        public async Task<IEnumerable<RegistroAuditoria>> ObtenerTodosAsync()
        {
            return await _dbSet
                .OrderByDescending(a => a.FechaRegistro)
                .ToListAsync();
        }

        public async Task AgregarAsync(RegistroAuditoria entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public Task ActualizarAsync(RegistroAuditoria entidad)
        {
            throw new NotSupportedException(
                "Los registros de auditoría no pueden ser modificados."
            );
        }

        public Task EliminarAsync(RegistroAuditoria entidad)
        {
            throw new NotSupportedException(
                "Los registros de auditoría no pueden ser eliminados."
            );
        }

        public async Task<IEnumerable<RegistroAuditoria>> ConsultarAsync(
            Guid? usuarioId,
            string? accion,
            string? entidadAfectada,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            var query = _dbSet.AsQueryable();

            if (usuarioId.HasValue)
            {
                query = query.Where(a => a.UsuarioId == usuarioId.Value);
            }

            if (!string.IsNullOrWhiteSpace(accion))
            {
                query = query.Where(a => a.Accion.Contains(accion.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(entidadAfectada))
            {
                query = query.Where(a => a.EntidadAfectada.Contains(entidadAfectada.Trim()));
            }

            if (fechaInicio.HasValue)
            {
                query = query.Where(a => a.FechaRegistro >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(a => a.FechaRegistro <= fechaFin.Value);
            }

            return await query
                .OrderByDescending(a => a.FechaRegistro)
                .ToListAsync();
        }
    }
}