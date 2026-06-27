using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Infrastructure.Services
{
    public class ServicioCorreoSMTP
    {
        private readonly IConfiguration _configuration;

        // Inyectamos IConfiguration para leer appsettings.json
        public ServicioCorreoSMTP(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarCorreoAsync(string destinatario, string asunto, string mensajeHtml)
        {
            // 1. Leer la configuración del servidor SMTP desde appsettings.json
            string host = _configuration["ConfiguracionSmtp:Host"] ?? string.Empty;
            int puerto = int.Parse(_configuration["ConfiguracionSmtp:Puerto"] ?? "587");
            string usuario = _configuration["ConfiguracionSmtp:Usuario"] ?? string.Empty;
            string contraseña = _configuration["ConfiguracionSmtp:Contraseña"] ?? string.Empty;
            string remitente = _configuration["ConfiguracionSmtp:Remitente"] ?? string.Empty;

            // 2. Configurar el cliente SMTP
            using var smtpClient = new SmtpClient(host, puerto)
            {
                Credentials = new NetworkCredential(usuario, contraseña),
                EnableSsl = true // Fundamental para Gmail/Outlook
            };

            // 3. Ensamblar el mensaje
            using var mailMessage = new MailMessage
            {
                From = new MailAddress(remitente, "Sistema SIGEBI"),
                Subject = asunto,
                Body = mensajeHtml,
                IsBodyHtml = true 
            };

            mailMessage.To.Add(destinatario);

           
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
