using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IUsuario : ReadOnly<Usuario>, Writer<Usuario>
    {
        Task<Usuario?> ObtenerPorIdentificacionAsync(string identificacion);
        Task<Usuario?> ObtenerConPerfilAsync(Guid id);
    }
}
