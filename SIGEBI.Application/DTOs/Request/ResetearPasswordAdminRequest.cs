using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Request
{
    public record ResetearPasswordAdminRequest
    {
        public string NuevaPassword { get; init; } = string.Empty;
    }
}
