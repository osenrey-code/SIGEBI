using SIGEBI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioPerfilLector : IBaseRepository<PerfilLector>
    {
        Task<PerfilLector> ObtenerPorTipoUsuarioAsync(string tipoUsuario);
    }
}
