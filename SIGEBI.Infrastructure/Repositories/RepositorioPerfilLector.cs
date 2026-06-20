using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistencia;
using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Application.Interfaces.Repositories;
namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioPerfilLector : IRepositorioPerfilLector
    {
        private readonly SIGEBIDbContext _context;

        public RepositorioPerfilLector(SIGEBIDbContext context)
        {
            _context = context;
        }

        public async Task<PerfilLector?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.PerfilesLectores.FindAsync(id);
        }

        public async Task AgregarAsync(PerfilLector perfil)
        {
            await _context.PerfilesLectores.AddAsync(perfil);
        }
    }
}
