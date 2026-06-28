using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public class AsignarPerfilLectorRequest
    {
        public Guid UsuarioEjecutorId { get; set; }
        public Guid UsuarioId { get; set; }
        public int LimitePrestamos { get; set; }
        public int DiasPrestamo { get; set; }
    }
}
