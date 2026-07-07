using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IUsuario : IRepositoryInmutable<Usuario>
    {
        Task ActualizarAsync(Usuario usuario);
        Task DesactivarUsuarioAsync(string IdUsuario);
        Task<Usuario?> ObtenerUsuarioPorIdentificacionAsync(string Identificacion);
        Task<bool> ExisteCorreoAsync(string correo);
        Task<Usuario?> ObtenerUsuarioConDetallesAsync(string Identifiacion);
        Task<IEnumerable<Usuario?>> ConsultarPorFiltrosAsync(string? nombre, string? tipoUsuario, string? estado);
    }
}
