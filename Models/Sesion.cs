using System;
using System.Collections.Generic;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Sesion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int ComputadoraId { get; set; }
        public int ProgramaId { get; set; }

        public virtual Computadora Computadora { get; set; } = null!;
        public virtual Programa Programa { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;
    }
}
