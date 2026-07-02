using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SIGEBI.Domain.Entities
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public virtual ICollection<RecursoBibliografico> Libros { get; set; } = new List<RecursoBibliografico>(); 
    }
}
