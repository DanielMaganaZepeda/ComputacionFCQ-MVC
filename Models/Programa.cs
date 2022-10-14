using System;
using System.Collections.Generic;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Programa
    {
        public Programa()
        {
            Reservacions = new HashSet<Reservacion>();
            Sesions = new HashSet<Sesion>();
            Salas = new HashSet<Sala>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public bool? Activo { get; set; }

        public virtual ICollection<Reservacion> Reservacions { get; set; }
        public virtual ICollection<Sesion> Sesions { get; set; }

        public virtual ICollection<Sala> Salas { get; set; }
    }
}
