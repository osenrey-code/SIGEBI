using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistencia;
using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Application.Interfaces.Repositories;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioUsuario : IUsuario
    {
        private readonly SIGEBIDbContext _context;

        public RepositorioUsuario(SIGEBIDbContext context)
        {
            _context = context; 
        }

        public async Task<Usuario?> ObtenerPorIdentificacionAsync(string identificacion)
        {
            return await _context.Usuarios
                .Include(u => u.PerfilLector)
                .FirstOrDefaultAsync(u => u.Identificacion == identificacion);
        }

        public async Task<Usuario?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Usuarios
                .Include(u => u.PerfilLector)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AgregarAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
        }
    }
}
