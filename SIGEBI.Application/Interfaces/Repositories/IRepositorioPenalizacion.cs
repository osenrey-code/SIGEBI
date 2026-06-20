using System;
using System.Collections.Generic;
using System.Text;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IRepositorioPenalizacion : ReadOnly<Penalizacion>, Writer<Penalizacion>
    {
        Task<IEnumerable<Penalizacion>> ObtenerPorPerfilLectorAsync(Guid perfilLectorId);

        Task<Penalizacion?> ObtenerActivaPorPerfilLectorAsync(Guid perfilLectorId);
    }
}
