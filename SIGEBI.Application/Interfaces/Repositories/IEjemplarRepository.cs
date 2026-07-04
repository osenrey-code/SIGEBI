using SIGEBI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.Interfaces.Repositories
{
    public interface IEjemplarRepository
    {
        Task<Ejemplar?> ObtenerPorIdAsync(int id);

        Task ActualizarAsync(Ejemplar ejemplar); **
    }
}
