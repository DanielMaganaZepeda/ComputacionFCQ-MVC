using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Carrera
    {
        public Carrera()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Usuario> Usuarios { get; set; }

        //Obtiene las carreras
        public static List<SelectListItem> GetCarreras()
        {
            using (var db = new ComputacionFCQContext())
            {
                return db.Carreras.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre
                }).ToList();
            }
        }
    }
}
