using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

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

        // --- Implementación de ReadOnly<Auditoria> ---
        public async Task<Auditoria?> ObtenerPorIdAsync(object id)
            => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<Auditoria>> ObtenerTodosAsync()
            => await _dbSet.ToListAsync();

        // --- Implementación de Writer<Auditoria> ---
        public async Task AgregarAsync(Auditoria entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public Task ActualizarAsync(Auditoria entidad)
            => throw new NotSupportedException("Los registros de auditoría son inmutables.");

        public Task EliminarAsync(Auditoria entidad)
            => throw new NotSupportedException("Los registros de auditoría no pueden ser eliminados.");

        // --- Implementación de IRepositorioAuditoria ---

        public async Task<IEnumerable<Auditoria>> ObtenerPorUsuarioAsync(Guid usuarioId)
        {
            return await _dbSet
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auditoria>> ObtenerPorRangoFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _dbSet
                .Where(a => a.FechaRegistro >= fechaInicio && a.FechaRegistro <= fechaFin)
                .OrderByDescending(a => a.FechaRegistro)
                .ToListAsync();
        }

        public async Task RegistrarAccionAsync(string usuarioId, string tipoAccion, string modulo, string detalle)
        {
            // Convertimos el string a Guid. Si falla, asigna Guid.Empty
            Guid usuarioGuid = Guid.TryParse(usuarioId, out var guid) ? guid : Guid.Empty;

            // Instanciamos usando tu constructor exacto
            var auditoria = new Auditoria(usuarioGuid, modulo, Guid.Empty, tipoAccion, detalle);

            await AgregarAsync(auditoria);
        }

        public async Task<IEnumerable<Auditoria>> ConsultarLogAsync(
            Guid? usuarioId,
            string? tipoAccion,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            IQueryable<Auditoria> query = _dbSet.AsQueryable();

            if (usuarioId.HasValue && usuarioId != Guid.Empty)
            {
                query = query.Where(a => a.UsuarioId == usuarioId.Value);
            }

            // Usamos !string.IsNullOrWhiteSpace para validar cadenas vacías de forma segura
            if (!string.IsNullOrWhiteSpace(tipoAccion))
            {
                query = query.Where(a => a.Accion == tipoAccion);
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
