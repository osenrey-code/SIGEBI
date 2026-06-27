using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.Interfaces.ext
{
    public interface IServicioCorreo : INotificador
    {
        Task EnviarCorreoAsync(string destinatario, string asunto, string mensajeHtml);
    }
}
