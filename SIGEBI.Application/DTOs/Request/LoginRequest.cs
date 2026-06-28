using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public class LoginRequest
    {
        public string UsuarioOCorreo { get; set; } = string.Empty;
        public string PassWord {  get; set; } = string.Empty;
    }
}
