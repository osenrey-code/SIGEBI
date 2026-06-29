using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Response;

public class LogAuditoriaResponse
{
    public Guid Id { get; set; }

    public Guid? UsuarioId { get; set; }

    public string Usuario { get; set; } = string.Empty;

    public string Accion { get; set; } = string.Empty;

    public string EntidadAfectada { get; set; } = string.Empty;

    public Guid? EntidadId { get; set; }

    public string Resultado { get; set; } = string.Empty;

    public string Detalle { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; }

    public string ValoresAnteriores { get; set; } = string.Empty;

    public string ValoresNuevos { get; set; } = string.Empty;
}