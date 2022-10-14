using System;
using System.Collections.Generic;

namespace ComputacionFCQ_MVC.Models
{
    public partial class VistaReservacione
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int SalaId { get; set; }
        public int ProgramaId { get; set; }
        public int? CantidadAlumnos { get; set; }
        public string? Curso { get; set; }
        public int? FrecuenciaId { get; set; }
    }
}
