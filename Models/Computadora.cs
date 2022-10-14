using System;
using System.Collections.Generic;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Computadora
    {
        public Computadora()
        {
            Reportes = new HashSet<Reporte>();
            Sesions = new HashSet<Sesion>();
        }

        public int Id { get; set; }
        public int SalaId { get; set; }
        public int Numero { get; set; }
        public bool? Funcional { get; set; }

        public virtual Sala Sala { get; set; } = null!;
        public virtual ICollection<Reporte> Reportes { get; set; }
        public virtual ICollection<Sesion> Sesions { get; set; }
    }
}
