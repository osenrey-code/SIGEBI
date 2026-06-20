using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface ReadOnly<T> where T : class
    {
        Task<T?> ObtenerPorIdAsync(object id);
        Task<IEnumerable<T>> ObtenerTodosAsync();
    }
}
