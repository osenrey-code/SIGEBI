using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request;

public class ConsultarLogAuditoriaRequest
{
    public Guid UsuarioEjecutorId { get; set; }

    public Guid? UsuarioId { get; set; }

    public string? Accion { get; set; }

    public string? EntidadAfectada { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }
}