using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Response
{
    public class ResultadoOperacionResponse
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;

        public static ResultadoOperacionResponse Ok(string mensaje)
        {
            return new ResultadoOperacionResponse
            {
                Exitoso = true,
                Mensaje = mensaje
            };
        }

        public static ResultadoOperacionResponse Error(string mensaje) 
        {
            return new ResultadoOperacionResponse
            {
                Exitoso = false,
                Mensaje = mensaje
            };
        }
    }


}
