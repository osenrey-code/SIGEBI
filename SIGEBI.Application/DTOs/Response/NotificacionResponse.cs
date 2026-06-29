using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Response;

public class NotificacionResponse
{
    public Guid Id { get; set; }

    public Guid? UsuarioDestinatarioId { get; set; }

    public string CorreoDestinatario { get; set; } = string.Empty;

    public string TipoEvento { get; set; } = string.Empty;

    public string Mensaje { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; }

    public string EstadoEnvio { get; set; } = string.Empty;
}