using System;
using System.Collections.Generic;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Reservacions = new HashSet<Reservacion>();
            Sesions = new HashSet<Sesion>();
        }

        public int Id { get; set; }
        public string Matricula { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public int CarreraId { get; set; }
        public string? Correo { get; set; }
        public bool? EsAlumno { get; set; }

        public virtual Carrera Carrera { get; set; } = null!;
        public virtual ICollection<Reservacion> Reservacions { get; set; }
        public virtual ICollection<Sesion> Sesions { get; set; }
    }
}
