using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IUsuario : IBaseRepository<Usuario>
    {
        Task RegistrarUsuarioAsync(RegistrarUsuarioRequest request);
        Task DesactivarUsuarioAsync(string IdUsuario);
        Task<UsuarioResponse?> ObtenerUsuarioPorIdentificacion(string identificacion);
        Task<IEnumerable<UsuarioResponse>> ListarTodosAsync(); 
    }
}
