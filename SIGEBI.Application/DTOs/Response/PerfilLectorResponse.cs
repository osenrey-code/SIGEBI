using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Response
{
    public class PerfilLectorResponse
    {
        public Guid PerfilLectorId { get; set; }
        public Guid UsuarioId { get; set; }
        public int LimitePrestamos { get; set; }
        public int DiasPrestamo { get; set; }
        public int PrestamosActivos { get; set; }
    }
}
