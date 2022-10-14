using System;
using System.Collections.Generic;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Administrador
    {
        public Administrador()
        {
            InverseCreadoPorNavigation = new HashSet<Administrador>();
        }

        public int Id { get; set; }
        public string Usuario { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string Contrasena { get; set; } = null!;
        public int CreadoPor { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }

        public virtual Administrador CreadoPorNavigation { get; set; } = null!;
        public virtual ICollection<Administrador> InverseCreadoPorNavigation { get; set; }
    }
}
