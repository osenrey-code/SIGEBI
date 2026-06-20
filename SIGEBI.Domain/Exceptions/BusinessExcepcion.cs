using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Domain.Exceptions
{
    internal class BusinessExcepcion : Exception
    {
        public BusinessExcepcion(string mensaje) : base(mensaje) { }
    }
}
