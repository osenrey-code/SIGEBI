using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Infrastructure.Persistence;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioPenalizacion : IRepositorioPenalizacion
    {
        private readonly SIGEBIDbContext _context;
        private readonly DbSet<Penalizacion> _dbSet;

        public RepositorioPenalizacion(SIGEBIDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Penalizacion>();
        }

        public async Task<Penalizacion?> ObtenerPorIdAsync(object id)
        {
            return await _dbSet
                .Include(p => p.PerfilLector)
                .Include(p => p.Prestamo)
                .FirstOrDefaultAsync(p => p.Id == (Guid)id);
        }

        public async Task<IEnumerable<Penalizacion>> ObtenerTodosAsync()
        {
            return await _dbSet
                .Include(p => p.PerfilLector)
                .Include(p => p.Prestamo)
                .ToListAsync();
        }

        public async Task AgregarAsync(Penalizacion entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Penalizacion entidad)
        {
            _dbSet.Update(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Penalizacion entidad)
        {
            _dbSet.Remove(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Penalizacion>> ObtenerPorPerfilLectorAsync(Guid perfilLectorId)
        {
            return await _dbSet
                .Include(p => p.PerfilLector)
                .Include(p => p.Prestamo)
                .Where(p => p.PerfilLectorId == perfilLectorId)
                .ToListAsync();
        }

        public async Task<Penalizacion?> ObtenerActivaPorPerfilLectorAsync(Guid perfilLectorId)
        {
            return await _dbSet
                .Include(p => p.PerfilLector)
                .Include(p => p.Prestamo)
                .FirstOrDefaultAsync(p =>
                    p.PerfilLectorId == perfilLectorId &&
                    p.Estado == EstadoPenalizacion.Activa);
        }

        public async Task<bool> ExisteActivaPorPerfilLectorAsync(Guid perfilLectorId)
        {
            return await _dbSet.AnyAsync(p =>
                p.PerfilLectorId == perfilLectorId &&
                p.Estado == EstadoPenalizacion.Activa);
        }

        public async Task<IEnumerable<Penalizacion>> ConsultarAsync(
            Guid? usuarioId,
            Guid? perfilLectorId,
            EstadoPenalizacion? estado,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            var query = _dbSet
                .Include(p => p.PerfilLector)
                .Include(p => p.Prestamo)
                .AsQueryable();

            if (usuarioId.HasValue)
            {
                query = query.Where(p => p.PerfilLector.UsuarioId == usuarioId.Value);
            }

            if (perfilLectorId.HasValue)
            {
                query = query.Where(p => p.PerfilLectorId == perfilLectorId.Value);
            }

            if (estado.HasValue)
            {
                query = query.Where(p => p.Estado == estado.Value);
            }

            if (fechaInicio.HasValue)
            {
                query = query.Where(p => p.FechaGeneracion >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(p => p.FechaGeneracion <= fechaFin.Value);
            }

            return await query.ToListAsync();
        }
    }
}