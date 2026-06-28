using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Interfaces.Repositories;
using SIGEBI.Domain.Entities;
using SIGEBI.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Infrastructure.Repositories
{
    public class RepositorioRecurso : RepositorioBase<RecursoBibliografico>, IRepositorioRecurso
    {
        public RepositorioRecurso(SIGEBIDbContext context) : base(context)
        {
            // El constructor simplemente le pasa el DbContext a la clase padre
        }

        public async Task<IEnumerable<RecursoBibliografico>> ConsultarCatalogoAsync(string titulo, string autor, string categoria)
        {
            // Iniciamos la consulta sin ir a la base de datos todavía
            var query = _dbSet.AsQueryable();

            // Vamos agregando los filtros dinámicamente solo si el usuario los proporcionó
            if (!string.IsNullOrWhiteSpace(titulo))
            {
                // Usamos Contains para que funcione como un "LIKE %titulo%" en SQL
                query = query.Where(r => r.Titulo.Contains(titulo));
            }

            if (!string.IsNullOrWhiteSpace(autor))
            {
                query = query.Where(r => r.Autor.Contains(autor));
            }

            if (!string.IsNullOrWhiteSpace(categoria))
            {
                // Aquí buscamos coincidencia exacta, pero puedes usar Contains si lo prefieres
                query = query.Where(r => r.Categoria == categoria);
            }

            // Finalmente, ejecutamos el viaje a la base de datos
            return await query.ToListAsync();
        }
    }
}
