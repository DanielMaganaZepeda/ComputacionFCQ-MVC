using System;
using System.Collections.Generic;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Reporte
    {
        public int Id { get; set; }
        public int ComputadoraId { get; set; }
        public string? Detalle { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaSolucion { get; set; }

        public virtual Computadora Computadora { get; set; } = null!;
    }
}
