using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IUsuario : IBaseRepository<Usuario>
    {
        Task<Usuario> ObtenerPorIdentificacionAsync(string identificacion);
    }
}
