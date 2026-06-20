using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioPenalizacion : IBaseRepository<PerfilLector>
    {
        Task<PerfilLector> ObtenerPorUsuarioIdAsync(Guid usuarioId);
    }
}
