using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using SIGEBI.Application.Interfaces.ext;

namespace SIGEBI.Infrastructure.Services
{
    public class ServicioCorreoSMTP : IServicioCorreo
    {
        private readonly IConfiguration _configuration;

        public ServicioCorreoSMTP(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarAsync(string destinatario, string asunto, string mensaje)
        {
            if (string.IsNullOrWhiteSpace(destinatario))
            {
                throw new ArgumentException("El destinatario es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(asunto))
            {
                throw new ArgumentException("El asunto es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(mensaje))
            {
                throw new ArgumentException("El mensaje es obligatorio.");
            }

            var host = _configuration["Smtp:Host"];
            var puertoTexto = _configuration["Smtp:Port"];
            var usuario = _configuration["Smtp:Username"];
            var password = _configuration["Smtp:Password"];
            var remitente = _configuration["Smtp:From"];

            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(puertoTexto) ||
                string.IsNullOrWhiteSpace(remitente))
            {
                throw new InvalidOperationException(
                    "La configuración SMTP está incompleta."
                );
            }

            var puerto = int.Parse(puertoTexto);

            using var cliente = new SmtpClient(host, puerto)
            {
                EnableSsl = true
            };

            if (!string.IsNullOrWhiteSpace(usuario) &&
                !string.IsNullOrWhiteSpace(password))
            {
                cliente.Credentials = new NetworkCredential(usuario, password);
            }

            using var correo = new MailMessage
            {
                From = new MailAddress(remitente),
                Subject = asunto,
                Body = mensaje,
                IsBodyHtml = false
            };

            correo.To.Add(destinatario);

            await cliente.SendMailAsync(correo);
        }
    }
}