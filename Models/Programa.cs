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

        public static string BuscarPorNombre(string nombre)
        {
            if (!Validaciones.Curso(nombre)) return "Ninguna sala dispone de este programa por el momento";
            try
            {
                using (var db = new ComputacionFCQContext())
                {
                    List<Sala> salas = db.Programas.Where(x => x.Nombre == nombre).FirstOrDefault().Salas.ToList();

                    if (salas.Count > 0)
                    {
                        string output = "Disponible en las salas: ";

                        foreach (var sala in salas)
                            output += $"{sala.Id}, ";
                        return output.Remove(output.Length - 2);
                    }
                    else
                        return "Ninguna sala dispone de este programa por el momento";
                }
            }
            catch { return "Error en Models.Programa.BuscarPorNombre, actualice la página y vuelva a intentarlo "; }
        }

        public static string GetProgramasJSON()
        {
            try
            {
                string json = "[";
                using (var db = new ComputacionFCQContext())
                {
                    List<string> programas = db.Programas.Where(x=>x.Activo==true && x.Nombre!="Sin programa").OrderBy(x => x.Nombre).Select(x => x.Nombre).ToList();

                    if (programas.Count == 0)
                        return "[]";

                    foreach(var programa in programas)
                    {
                        json += $"'{programa}', ";
                    }
                }
                return json + "]";
            }
            catch { return "[]"; }
        }

        public static void EliminarPrograma(string id)
        {
            try
            {
                using (var db = new ComputacionFCQContext())
                {
                    db.Database.ExecuteSqlRaw("exec SP_EliminarPrograma @p0", parameters: new[] { id });
                }
            }
            catch { }
        }

        public static string? AgregarPrograma(string nombre)
        {
            try
            {
                using (var db = new ComputacionFCQContext())
                {
                    if (!Validaciones.Curso(nombre)) return "Debe ingresarse un nombre del programa válido";

                    if (db.Programas.Where(x => x.Nombre == nombre && x.Activo == true).Count() > 0)
                        return "Ya existe un programa registrado con ese nombre";

                    db.Database.ExecuteSqlRaw("exec SP_AgregarPrograma @p0", parameters: new[] { nombre });
                }
                return null;
            }
            catch { return "Error desconocido en Models.Programa.AgregarPrograma, actualice la pagina y vuelva a intentarlo"; }
        }

        //Llega un array con el id de los cambios que se realizaran en formato : "{sala}_{programa}"
        public static void ActualizarProgramas(string[] ids)
        {
            try
            {
                using (var db = new ComputacionFCQContext())
                {
                    foreach (var id in ids)
                    {
                        db.Database.ExecuteSqlRaw("exec SP_ActualizarProgramaSala @p0, @p1",
                            parameters: new[] { id.Split('_')[1], id.Split('_')[0] });
                    }
                }
            }
            catch { }
        }

        //Devuelve todos los programas que esten activos en al menos una sala ordenados alfabeticamente
        public static List<Programa> GetProgramas()
        {
            try
            {
                using (var db = new ComputacionFCQContext())
                {
                    var lista = db.Programas.Where(x => x.Activo == true && x.Nombre!="Sin programa").OrderBy(x => x.Nombre).ToList();
                    //No se porque pero si quitamos este for each genera un error, quiero pensar que es para que el LazyLoading reconozca los valores de sala
                    foreach (Programa programa in lista)
                    {
                        foreach (var sala in programa.Salas)
                        {
                        }
                    }
                    return lista;
                }
            }
            catch { return new List<Programa>(); }
        }
    }
}
