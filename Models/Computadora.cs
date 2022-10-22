using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Computadora
    {
        public Computadora()
        {
            Reportes = new HashSet<Reporte>();
            Sesions = new HashSet<Sesion>();
        }

        public int Id { get; set; }
        public int SalaId { get; set; }
        public int Numero { get; set; }
        public bool? Funcional { get; set; }

        public virtual Sala Sala { get; set; } = null!;
        public virtual ICollection<Reporte> Reportes { get; set; }
        public virtual ICollection<Sesion> Sesions { get; set; }

        //Retorna string en formato JSON con todos los reportes sin solucion en orden por fecha
        public static string GetReportes()
        {
            try
            {
                string json = "[";
                using (var db = new ComputacionFCQContext())
                {
                    var reportes = db.Reportes.Where(x => x.FechaSolucion == null).ToList();

                    foreach (var reporte in reportes)
                    {
                        json += "{";
                        json += $"sala: {reporte.Computadora.SalaId},";
                        json += $"numero: {reporte.Computadora.Numero},";
                        json += $"fecha: '{reporte.Detalle}',";
                        json += $"detalle: '{Fecha.ParseFecha(reporte.FechaCreacion)}',";
                        json += "},";
                    }
                }
                return json += "]";
            }
            catch { return "[]"; }
        }

        public static int SolucionarReporte(string reporte_id, int sala_id, int numero)
        {
            try
            {
                using (var db = new ComputacionFCQContext())
                {
                    db.Database.ExecuteSqlRaw("exec SP_SolucionarReporte @p0", parameters: new[] { reporte_id });
                    return db.Reportes.Where(x => x.Computadora.SalaId == sala_id && x.Computadora.Numero == numero && x.FechaSolucion == null).Count();
                }
            }
            catch { return 0; }
        }

        public static string? AgregarReporte(string sala_id, string numero, string detalle)
        {
            try
            {
                if (!Validaciones.Curso(detalle)) return "Debe introducirse una descripción del reporte válida";

                using (var db = new ComputacionFCQContext())
                {
                    db.Database.ExecuteSqlRaw("exec SP_AgregarReporte @p0, @p1, @p2",
                        parameters: new[] { sala_id, numero, detalle });
                }
                return null;
            }
            catch { return "Error desconocido en Models.Computadora.AgregarReporte, reinicie la pagina y vuelva a intentar"; }
        }

        //Retorna string en formato JSON con el estado de las computadoras por sala indicada
        public static string? GetComputadoras(int sala_id)
        {
            try
            {
                string json = "[";
                using (var db = new ComputacionFCQContext())
                {
                    var sala = db.Salas.Find(sala_id);

                    foreach (var comp in sala.Computadoras)
                    {
                        json += "{";
                        json += $"numero: {comp.Numero},";
                        if (comp.Funcional == true)
                            json += "disponible : true,";
                        else
                            json += "disponible : false,";

                        json += "},";
                    }
                }
                return json + "]";
            }
            catch { return "[]"; }
        }

        //Retorna string en formato JSON con el historial de reportes de una computadora
        public static string GetComputadoraDetalle(int sala_id, int numero)
        {
            try
            {
                string json = "[";
                using (var db = new ComputacionFCQContext())
                {
                    List<Reporte> reportes = db.Reportes.Where(x => x.Computadora.SalaId == sala_id && x.Computadora.Numero == numero).OrderBy(x => x.FechaSolucion).ToList();

                    foreach (var reporte in reportes)
                    {
                        json += "{";
                        json += $"detalle: '{reporte.Detalle}',";
                        json += $"id: {reporte.Id},";
                        json += $"fecha_creacion: '{Fecha.ParseFecha(reporte.FechaCreacion)}',";
                        json += reporte.FechaSolucion == null ? "fecha_solucion: ''," : $"fecha_solucion: '{Fecha.ParseFecha(reporte.FechaSolucion.Value)}',";
                        json += "},";
                    }
                }
                return json + "]";
            }
            catch { return "[]"; }
        }
    }
}
