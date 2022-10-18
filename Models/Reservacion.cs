using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ComputacionFCQ_MVC.Models;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Reservacion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int SalaId { get; set; }
        public int ProgramaId { get; set; }
        public int? CantidadAlumnos { get; set; }
        public bool? Activa { get; set; }
        public string? Curso { get; set; }
        public int? FrecuenciaId { get; set; }

        public virtual Frecuencium? Frecuencia { get; set; }
        public virtual Programa Programa { get; set; } = null!;
        public virtual Sala Sala { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;

        public static string GetReservacionPorId(int id)
        {
            List<string> dias = new List<string> { "Lu", "Ma", "Mi", "Ju", "Vi", "Sa" };
            string json = "{";
            using (var db = new ComputacionFCQContext())
            {
                var reservacion = db.Reservacions.Find(id);

                json += $"matricula: {db.Usuarios.Find(reservacion.UsuarioId).Matricula}, ";
                json += $"cantidad_alumnos: {reservacion.CantidadAlumnos}, ";
                json += $"sala_id: {reservacion.SalaId}, ";
                json += $"programa: '{db.Programas.Find(reservacion.ProgramaId).Nombre}', ";

                //Si es unica
                if (reservacion.FrecuenciaId == null)
                {
                    DateTime dt = reservacion.FechaInicio.Value;

                    json += "es_unica: true, ";
                    json += $"curso: '{reservacion.Curso}', ";
                    json += $"fecha: '{dt.Day}/{Fecha.GetMes(dt.Month)}/{dt.Year}', ";
                    json += $"hi: '{dt.Hour}', ";
                    json += $"hf: '{reservacion.FechaFin.Value.Hour}'";

                }
                //Si es frecuencial
                else
                {
                    Frecuencium frecuencia = db.Frecuencia.Find(reservacion.FrecuenciaId);

                    json += $"totales: {db.Reservacions.Where(x=>x.FrecuenciaId==frecuencia.Id).Count()},";
                    json += $"restantes: {db.Reservacions.Where(x=>x.FrecuenciaId==frecuencia.Id && x.FechaInicio.Value > DateTime.Now).Count()},";
                    json += $"fecha: '{reservacion.FechaInicio.Value.ToString("dd-MM-yyyy")}',";

                    json += "es_unica: false, ";
                    json += $"curso: '{frecuencia.Curso}', ";

                    DateTime dt = frecuencia.PeriodoFin.Value;
                    json += $"periodo_fin: '{dt.Day}/{Fecha.GetMes(dt.Month)}/{dt.Year}', ";

                    dt = frecuencia.PeriodoInicio.Value;
                    json += $"periodo_inicio: '{dt.Day}/{Fecha.GetMes(dt.Month)}/{dt.Year}', ";

                    //Se envia arreglo de los dias que se presentan
                    DateTime dt_lim = dt.AddDays(7);
                    json += "dias: [";
                    while(dt.Date < dt_lim.Date)
                    {
                        //Se busca una reservacion de la misma frecuencia en el dia seleccionado
                        var aux = db.Reservacions.Where(x => x.FrecuenciaId == frecuencia.Id && x.FechaInicio.Value.Date == dt.Date).FirstOrDefault();
                        if (aux != null)
                        {
                            json += "{";
                            json += $"id: '{dias[(int)dt.DayOfWeek - 1]}',";
                            json += $"hi: '{aux.FechaInicio.Value.Hour}',";
                            json += $"hf: '{aux.FechaFin.Value.Hour}'";
                            json += "}, ";
                        }
                        dt = dt.AddDays(1);
                    }
                    json += "]";
                }
            }
            json += "}";
            return json;
        }

        public static List<ReservacionKeys> GetReservacionesSemana(int sala, DateTime dt)
        {
            List<string> dias = new List<string> { "Lu", "Ma", "Mi", "Ju", "Vi", "Sa" };
            using (var db = new ComputacionFCQContext())
            {
                var output = new List<ReservacionKeys>();
                var lista = db.Reservacions.Where(x => x.Activa == true && x.SalaId == sala && x.FechaInicio > dt && x.FechaInicio < dt.AddDays(6)).ToList();

                foreach(Reservacion rsv in lista)
                {
                    var TargetIds = new List<string>();

                    for (int hora = rsv.FechaInicio.Value.Hour; hora < rsv.FechaFin.Value.Hour; hora++)
                        TargetIds.Add(dias[((int)rsv.FechaInicio.Value.DayOfWeek) - 1] + hora);

                    output.Add(new ReservacionKeys(rsv.Id, rsv.FrecuenciaId == null ? rsv.Curso : db.Frecuencia.Find(rsv.FrecuenciaId).Curso,
                        db.Usuarios.Find(rsv.UsuarioId).Nombre + " " + db.Usuarios.Find(rsv.UsuarioId).Apellidos, TargetIds, rsv.FrecuenciaId==null ? true : false));
                }

                return output;
            }
        }
    }

    public class ReservacionKeys
    {
        public ReservacionKeys(int id, string curso, string nombre, List<string> targetIds, bool unico)
        {
            Id = id;
            Curso = curso;
            Nombre = nombre;
            TargetIds = targetIds;

            if (unico)
                BackgroundColor = "bisque";
            else
                BackgroundColor = "lavender";
        }

        public int Id { get; set; }
        public string Curso { get; set;}
        public string Nombre { get; set; }
        public List<string> TargetIds { get; set; }
        public string BackgroundColor { get; set; }

    }
}
