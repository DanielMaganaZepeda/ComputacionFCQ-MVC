using Microsoft.AspNetCore.Mvc;
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

        public static bool ValidarMatricula(string matricula)
        {
            if (matricula == "" || matricula==null) return false;
            foreach (char c in matricula)
                if (!char.IsDigit(c)) return false;
            return true;
        }

        public static Usuario? GetUsuarioPorMatricula(string matricula)
        {
            using (var db = new ComputacionFCQContext())
            {
                if (db.Usuarios.Where(x => x.Matricula == matricula).FirstOrDefault() != null)
                {
                    return db.Usuarios.Where(x => x.Matricula == matricula).First();
                }
                return null;
            }
        }
    }
}
