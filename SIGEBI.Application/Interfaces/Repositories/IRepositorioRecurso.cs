using SIGEBI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioRecurso : IBaseRepository<RecursoBibliografico>
    {
        Task<IEnumerable<RecursoBibliografico>> ConsultarCatalogoAsync(string titulo, string autor, string categoria);
    }
}
