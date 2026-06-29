using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request;

public class ConsultarPenalizacionesRequest
{
    public Guid UsuarioEjecutorId { get; set; }

    public Guid? UsuarioId { get; set; }

    public Guid? PerfilLectorId { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }
}