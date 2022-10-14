using System;
using System.Collections.Generic;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Sala
    {
        public Sala()
        {
            Computadoras = new HashSet<Computadora>();
            Reservacions = new HashSet<Reservacion>();
            Programas = new HashSet<Programa>();
        }

        public int Id { get; set; }

        public virtual ICollection<Computadora> Computadoras { get; set; }
        public virtual ICollection<Reservacion> Reservacions { get; set; }

        public virtual ICollection<Programa> Programas { get; set; }
    }
}
