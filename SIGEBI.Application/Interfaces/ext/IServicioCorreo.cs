using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.Interfaces.ext
{
    public interface IServicioCorreo : INotificador
    {
        public async Task EnviarAsync(string destino, string mensaje)
        {

        }
    }
}
