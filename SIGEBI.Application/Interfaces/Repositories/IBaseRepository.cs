using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> ObtenerporIdAsync(object id);
        Task<IEnumerable<T>> ObtenerTodosAsync();
        Task AgregarAsync(T entidad);
        Task ActualizarAsync(T entidad);
        Task EliminarAsync(T entidad);
    }
}
