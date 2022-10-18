using System;
using System.Collections.Generic;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Fecha
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateTime? Fecha1 { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string id { get; set; }

        public static string GetDia(int dia)
        {
            List<string> dias = new List<string> { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sábado" };
            return dias[dia - 1];
        }

        public static string GetMes(int mes)
        {
            List<string> meses = new List<string> { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            return meses[mes - 1];
        }

        public static Fecha? GetInicio(string id)
        {
            using (var db = new ComputacionFCQContext())
            {
                Fecha? fecha = db.Fechas.Where(x=>x.Nombre=="inicio").FirstOrDefault();
                fecha.id = id;
                return fecha;
            }
        }
        
        public static Fecha? GetFinal(string id)
        {
            using (var db = new ComputacionFCQContext())
            {
                Fecha? fecha = db.Fechas.Where(x => x.Nombre == "final").FirstOrDefault();
                fecha.id = id;
                return fecha;
            }
        }
    }
}
