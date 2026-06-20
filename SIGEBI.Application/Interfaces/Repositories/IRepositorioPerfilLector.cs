using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;


namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioPerfilLector : ReadOnly<PerfilLector>
    {
        Task<IEnumerable<PerfilLector>> ObtenerPorTipoUsuarioAsync(TipoUsuario tipo);
    }
}
