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

        public static string? ValidarUsuario(string usuario, string contrasena)
        {
            try
            {
                if (usuario == null || contrasena == null || usuario == "" || contrasena == null)
                    return null;
                if (usuario.Contains('\'') || contrasena.Contains('\''))
                    return null;

                using (var db = new ComputacionFCQContext())
                {
                    Administrador admin = db.Administradors.Where(x => x.Usuario == usuario && x.Contrasena == contrasena).FirstOrDefault();

                    if (admin != null)
                        return admin.Id.ToString();
                    else
                        return null;
                }
            }
            catch { return null; }
        }
    }
}
