using System;
using System.Collections.Generic;

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
