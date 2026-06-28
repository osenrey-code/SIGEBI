using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Application.DTOs.Response
{
    public class ResultadoOperacionResponse<T>
    {
        public bool Exitoso { get; set;  }
        public string Mensaje { get; set; } = string.Empty;
        public T? Datos { get; set; }

        public static ResultadoOperacionResponse<T> Ok(string mensaje, T datos)
        {
            return new ResultadoOperacionResponse<T>
            {
                Exitoso = true,
                Mensaje = mensaje,
                Datos = datos
            };
        }

        public static ResultadoOperacionResponse<T> Error(string mensaje)
        {
            return new ResultadoOperacionResponse<T>
            {
                Exitoso = false,
                Mensaje = mensaje,
                Datos = default
            };
        }
    }
}
