using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string mensaje) : base(mensaje) { }
    }
}
