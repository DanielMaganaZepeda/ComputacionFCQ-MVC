using Microsoft.EntityFrameworkCore;

namespace ComputacionFCQ_MVC.Models
{
    public partial class Fecha
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateTime? Fecha1 { get; set; }

        public static string? ActualizarFechas(string inicio, string fin)
        {
            DateTime periodo_inicio = ParseFecha(inicio);
            DateTime periodo_fin = ParseFecha(fin);

            if (periodo_inicio > periodo_fin) return "La fecha de inicio no puede ser futura a la fecha de fin";
            if (periodo_inicio == periodo_fin) return "Las fechas deben ser distintas";

            using (var db = new ComputacionFCQContext())
            {
                db.Database.ExecuteSqlRaw("exec SP_ActualizarFechas @p0, @p1", 
                    parameters: new[] { periodo_inicio.ToString("yyyy-MM-dd 00:00:00"), periodo_fin.ToString("yyyy-MM-dd 23:00:00") });
            }
            return null;
        }

        //Recibe fecha en string con el formato "dd/{Nombre del mes}/yyyy" y lo retorna como DateTime
        public static DateTime ParseFecha(string fecha)
        {
            return new DateTime(Convert.ToInt32(fecha.Split('/')[2]), GetMes(fecha.Split('/')[1]), Convert.ToInt32(fecha.Split('/')[0]));
        }

        //Recibe fecha en DateTime y retorna en formato "dd/{Nombre del mes}/yyyy"
        public static string ParseFecha(DateTime fecha)
        {
            return $"{fecha.Day}/{GetMes(fecha.Month)}/{fecha.Year}";
        }

        //Recibe (int)DayOfWeek y retorna el nombre del dia
        public static string GetDia(int dia)
        {
            List<string> dias = new List<string> { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sábado" };
            return dias[dia - 1];
        }

        //Recibe nombre del mes y retorna (int)Mes
        public static int GetMes(string mes)
        {
            List<string> meses = new List<string> { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            return meses.IndexOf(mes)+1;
        }

        //Recibe (int)Mes y retorna nombre del mes
        public static string GetMes(int mes)
        {
            List<string> meses = new List<string> { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            return meses[mes - 1];
        }

        //Retorna la fecha de inicio del semestre, ID CONTIENE EL ID DEL DATEPICKER
        public static Fecha GetInicio(string id)
        {
            using (var db = new ComputacionFCQContext())
            {
                Fecha? fecha = db.Fechas.Where(x=>x.Nombre=="inicio").FirstOrDefault();
                fecha.Nombre = id;
                return fecha;
            }
        }
        
        //Retorna la fecha de fin del semestre, ID CONTIENE EL ID DEL DATEPICKER
        public static Fecha GetFinal(string id)
        {
            using (var db = new ComputacionFCQContext())
            {
                Fecha? fecha = db.Fechas.Where(x => x.Nombre == "final").FirstOrDefault();
                fecha.Nombre = id;
                return fecha;
            }
        }
    }
}
