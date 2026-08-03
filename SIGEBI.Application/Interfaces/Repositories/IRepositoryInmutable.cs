using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositoryInmutable<T> where T : class
    {
       public Task<T?> ObtenerporIdAsync(object id);
       public Task<IEnumerable<T>> ObtenerTodosAsync();
       public Task AgregarAsync(T entidad);
    }
}
