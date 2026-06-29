using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request;

public class ConsultarPenalizacionesActivasRequest
{
    public Guid UsuarioEjecutorId { get; set; }

    public Guid? UsuarioId { get; set; }

    public Guid? PerfilLectorId { get; set; }
}