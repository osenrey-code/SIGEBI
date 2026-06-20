using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.Interfaces.ext
{
    public interface INotificador
    {
        Task EnviarAsync(string destino, string mensaje);
    }
}
