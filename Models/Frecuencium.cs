using System;
using System.Collections.Generic;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Frecuencium
    {
        public Frecuencium()
        {
            Reservacions = new HashSet<Reservacion>();
        }

        public int Id { get; set; }
        public string? Curso { get; set; }
        public DateTime? PeriodoInicio { get; set; }
        public DateTime? PeriodoFin { get; set; }

        public virtual ICollection<Reservacion> Reservacions { get; set; }
    }
}
