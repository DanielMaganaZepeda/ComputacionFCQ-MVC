using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

        public static void EliminarPrograma(string id)
        {
            using (var db = new ComputacionFCQContext())
            {
                db.Database.ExecuteSqlRaw("exec SP_EliminarPrograma @p0", parameters: new[] { id });
            }
        }

        public static string? AgregarPrograma(string nombre)
        {
            using (var db = new ComputacionFCQContext())
            {
                if (!Validaciones.Curso(nombre)) return "Debe ingresarse un nombre del programa válido";

                if (db.Programas.Where(x => x.Nombre == nombre && x.Activo==true).Count() > 0)
                    return "Ya existe un programa registrado con ese nombre";

                db.Database.ExecuteSqlRaw("exec SP_AgregarPrograma @p0", parameters: new[] { nombre });
            }
            return null;
        }

        //Llega un array con el id de los cambios que se realizaran en formato : "{sala}_{programa}"
        public static void ActualizarProgramas(string[] ids)
        {
            using (var db = new ComputacionFCQContext())
            {
                foreach(var id in ids)
                {
                    db.Database.ExecuteSqlRaw("exec SP_ActualizarProgramaSala @p0, @p1",
                        parameters: new[] { id.Split('_')[1], id.Split('_')[0] });
                }
            }
        }

        //Devuelve todos los programas que esten activos en al menos una sala ordenados alfabeticamente
        public static List<Programa> GetProgramas()
        {
            using (var db = new ComputacionFCQContext())
            {
                var lista = db.Programas.Where(x => x.Activo==true).OrderBy(x => x.Nombre).ToList();
                //No se porque pero si quitamos este for each genera un error, quiero pensar que es para que el LazyLoading reconozca los valores de sala
                foreach(Programa programa in lista)
                {
                    foreach(var sala in programa.Salas)
                    {
                    }
                }
                return lista;
            }
        }
    }
}
