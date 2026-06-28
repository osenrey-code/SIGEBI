using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using SIGEBI.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioPerfilLector : IRepositorioPerfilLector
    {
        private readonly SIGEBIDbContext _context;
        private readonly DbSet<PerfilLector> _dbSet;

        public RepositorioPerfilLector(SIGEBIDbContext context)
        {
            _context = context;
            _dbSet = context.Set<PerfilLector>();
        }

        // --- IReaderRepository (Lectura Obligatoria) ---

        public async Task<PerfilLector?> ObtenerPorIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<PerfilLector>> ObtenerTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // --- IRepositorioPerfilLector (Específicos) ---

        public async Task<PerfilLector?> ObtenerPorUsuarioIdAsync(Guid usuarioId)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
        }

        public async Task<IEnumerable<PerfilLector>> ObtenerPorTipoUsuarioAsync(TipoUsuario tipo)
        {
            return await _dbSet
               .Join(
                   _context.Usuarios,
                   perfil => perfil.UsuarioId,
                   usuario => usuario.Id,
                   (perfil, usuario) => new { perfil, usuario }
               )
               .Where(x => x.usuario.Tipo == tipo)
               .Select(x => x.perfil)
               .ToListAsync();
        }
    }
}
