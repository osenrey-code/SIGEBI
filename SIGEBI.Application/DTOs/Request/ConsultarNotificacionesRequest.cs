using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request;

public class ConsultarNotificacionesRequest
{
    public Guid UsuarioEjecutorId { get; set; }

    public Guid? UsuarioDestinatarioId { get; set; }

    public string? TipoEvento { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }
}