using Microsoft.EntityFrameworkCore;

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

        public static string GetReservacionesFiltradas(string desde, string hasta, int[] salas, string[] tipos, string?[] estados)
        {
            try
            {
                string json = "[";
                using (var db = new ComputacionFCQContext())
                {
                    DateTime inicio;
                    if (desde == "Seleccionar fecha" || desde == null)
                        inicio = DateTime.Now;
                    else
                        inicio = Fecha.ParseFecha(desde);

                    DateTime fin;
                    if (hasta == "Seleccionar fecha" || desde == null)
                        fin = db.Reservacions.OrderByDescending(x => x.FechaInicio).First().FechaInicio.Value;
                    else
                        fin = Fecha.ParseFecha(hasta);

                    //Obtenemos primero todas las reservaciones UNICAS dentro del rango de fecha
                    List<Reservacion> reservaciones = db.Reservacions.Where(x => x.FechaInicio >= inicio && x.FechaFin <= fin &&
                    salas.Contains(x.SalaId) && x.FrecuenciaId == null).ToList();

                    //Obtenemos las frecuencias dentro del rango de fechas
                    List<int?> frecuencias_ids = db.Reservacions.Where(x => x.FechaInicio >= inicio && x.FechaFin <= fin &&
                    salas.Contains(x.SalaId) && x.FrecuenciaId != null).Select(x => x.FrecuenciaId).Distinct().ToList();

                    //Le agregamos a la lista de unicas la primera reservacion frecuencial de cada frecuencia 
                    foreach (var frecuencia_id in frecuencias_ids)
                        reservaciones.Add(db.Reservacions.Where(x => x.FrecuenciaId == frecuencia_id).First());

                    //Aplicamos el filtro de Tipo
                    if (tipos.Length == 1 && tipos[0] == "Unicas")
                        reservaciones.RemoveAll(x => x.FrecuenciaId != null);
                    if (tipos.Length == 1 && tipos[0] == "Frecuencias")
                        reservaciones.RemoveAll(x => x.FrecuenciaId == null);

                    //Aplicamos el filtro de estado
                    //Se considera una frecuencia cancelada si NO EXISTE RESERVACION CON LA FRECUENCIA INDICADA ACTIVA (Count()==0)
                    if (estados.Length == 1 && estados[0] == "Activas")
                    {
                        reservaciones.RemoveAll(x => x.Activa == false && x.FrecuenciaId != null);
                        foreach (var frecuencia_id in frecuencias_ids)
                        {
                            if (db.Reservacions.Where(x => x.FrecuenciaId == frecuencia_id && x.Activa == true).Count() == 0)
                                reservaciones.RemoveAll(x => x.FrecuenciaId == frecuencia_id);
                        }
                    }
                    if (estados.Length == 1 && estados[0] == "Canceladas")
                    {
                        reservaciones.RemoveAll(x => x.FrecuenciaId == null && x.Activa == true);
                        foreach (var frecuencia_id in frecuencias_ids)
                        {
                            if (db.Reservacions.Where(x => x.FrecuenciaId == frecuencia_id && x.Activa == true).Count() > 0)
                                reservaciones.RemoveAll(x => x.FrecuenciaId == frecuencia_id);
                        }
                    }

                    //Finalmente en la lista quedan las reservaciones unicas y frecuenciales ordenadas
                    reservaciones = reservaciones.OrderBy(x => x.FechaInicio).ToList();

                    foreach (Reservacion rsv in reservaciones)
                    {
                        json += "{";
                        json += $"id: {rsv.Id},";
                        json += $"sala_id: {rsv.SalaId},";
                        json += $"nombre: '{rsv.Usuario.Nombre} {rsv.Usuario.Apellidos}',";

                        if (rsv.FrecuenciaId == null)
                        {
                            json += $"tipo: 'Unica',";
                            json += $"curso: '{rsv.Curso}',";

                            if (rsv.FechaFin <= DateTime.Now && rsv.Activa == true)
                                json += "estado: 'Finalizada',";
                            else
                                json += rsv.Activa == true ? "estado: 'Activa'," : "estado: 'Cancelada',";

                            json += $"fecha: '{Fecha.ParseFecha(rsv.FechaInicio.Value)}',";
                            json += $"hi: '{rsv.FechaInicio.Value.ToString("HH")}',";
                            json += $"hf: '{rsv.FechaFin.Value.ToString("HH")}'";
                        }
                        else
                        {
                            json += "tipo: 'Frecuencia',";
                            json += $"curso: '{rsv.Frecuencia.Curso}',";

                            if (db.Reservacions.Where(x => x.FrecuenciaId == rsv.FrecuenciaId && x.Activa == true).Count() == 0)
                                json += "estado: 'Cancelada',";
                            else
                            {
                                if (db.Reservacions.Where(x => x.FrecuenciaId == rsv.FrecuenciaId).OrderByDescending(x => x.FechaInicio).First().FechaInicio.Value <= DateTime.Now)
                                    json += "estado: 'Finalizada',";
                                else
                                    json += "estado: 'Activa',";
                            }

                            json += $"periodo_inicio: '{Fecha.ParseFecha(rsv.Frecuencia.PeriodoInicio.Value)}',";
                            json += $"periodo_fin: '{Fecha.ParseFecha(rsv.Frecuencia.PeriodoFin.Value)}'";
                        }

                        json += "},";
                    }
                }
                return json + "]";
            }
            catch { return "[]"; }
        }

        //Agregar reservacion unica
        public static void AgregarReservacion(string matricula, string curso, string cantidad, int sala_id, string programa, string fecha, int hi, int hf)
        {
            try
            {
                using (var db = new ComputacionFCQContext())
                {
                    int usuario_id = db.Usuarios.Where(x => x.Matricula == matricula).First().Id;
                    DateTime inicio = Fecha.ParseFecha(fecha);
                    DateTime fin = inicio.AddHours(hf);
                    inicio = inicio.AddHours(hi);
                    int programa_id = db.Programas.Where(x => x.Nombre == programa).First().Id;

                    db.Database.ExecuteSqlRaw("exec SP_AgregarReservacion @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7",
                        parameters: new[] {usuario_id.ToString(), inicio.ToString("yyyy-MM-dd HH:00:00"), fin.ToString("yyyy - MM - dd HH: 00:00"),
                                       sala_id.ToString(), programa_id.ToString(), cantidad, curso, null});

                }
            }
            catch { }
        }

        //Agregar reservacion frecuencial
        public static int AgregarReservacion(string matricula, string curso, string cantidad, int sala_id, string programa,
            string periodo_inicio, string periodo_fin, string[] dias)
        {
            try
            {
                int contador = 0;
                int[,] lista = new int[dias.Length, 3];

                //Desglozamos el string formateado en 3 columnas de ints 
                for (int i = 0; i < dias.Length; i++)
                {
                    lista[i, 0] = Convert.ToInt32(dias[i].Split('-')[0]); //DayOfWeek (Sunday=0, Monday=1...)
                    lista[i, 1] = Convert.ToInt32(dias[i].Split('-')[1]); //Hora inicio
                    lista[i, 2] = Convert.ToInt32(dias[i].Split('-')[2]); //Hora fin
                }

                using (var db = new ComputacionFCQContext())
                {
                    DateTime inicio = Fecha.ParseFecha(periodo_inicio);
                    DateTime fin = Fecha.ParseFecha(periodo_fin);
                    DateTime aux_inicio, aux_fin;

                    db.Database.ExecuteSqlRaw("exec SP_AgregarFrecuencia @p0, @p1, @p2",
                        parameters: new[] { curso, inicio.ToString("yyyy-MM-dd HH:00:00"), fin.ToString("yyyy-MM-dd HH:00:00") });

                    int frecuencia_id = db.Frecuencia.OrderByDescending(x => x.Id).First().Id;
                    int usuario_id = db.Usuarios.Where(x => x.Matricula == matricula).First().Id;
                    int programa_id = db.Programas.Where(x => x.Nombre == programa).First().Id;

                    //Avanzando semana por semana
                    while (inicio <= fin)
                    {
                        //Por cada dia seleccionado validamos que no interfiera
                        for (int i = 0; i < dias.Length; i++)
                        {
                            if (lista[i, 0] - (int)inicio.DayOfWeek >= 0)
                                aux_inicio = inicio.AddDays(lista[i, 0] - (int)inicio.DayOfWeek);
                            else
                                aux_inicio = inicio.AddDays(7 - (int)inicio.DayOfWeek + lista[i, 0]);

                            aux_fin = aux_inicio.AddHours(lista[i, 2]);
                            aux_inicio = aux_inicio.AddHours(lista[i, 1]);

                            if (aux_inicio >= fin) continue;

                            db.Database.ExecuteSqlRaw("exec SP_AgregarReservacion @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7",
                                parameters: new[] {usuario_id.ToString(), aux_inicio.ToString("yyyy-MM-dd HH:00:00"), aux_fin.ToString("yyyy-MM-dd HH:00:00"),
                                               sala_id.ToString(), programa_id.ToString(), cantidad.ToString(), null, frecuencia_id.ToString()});
                            contador++;

                        }
                        inicio = inicio.AddDays(7);
                    }
                }
                return contador;
            }
            catch { return 0; }
        }

        public static void CancelarEvento(string id)
        {
            try
            {
                using (var db = new ComputacionFCQContext())
                {
                    db.Database.ExecuteSqlRaw("exec SP_EliminarEvento @p0", parameters: new[] { id });
                }
            }
            catch { }
        }

        public static void CancelarFrecuencia(int id)
        {
            try
            {
                using (var db = new ComputacionFCQContext())
                {
                    int? frecuencia_id = db.Reservacions.Find(id).FrecuenciaId;
                    db.Database.ExecuteSqlRaw("exec SP_EliminarFrecuencia @p0", parameters: new[] { frecuencia_id.ToString() });
                }
            }
            catch { }
        }

        //Valida que la reservacion UNICA pueda ser creada (retorna: null si es valida, string con mensaje de error si no)
        public static string? ValidarReservacion(string curso, string cantidad, int sala_id, string fecha, int hi, int hf)
        {
            try
            {
                if (!Validaciones.Curso(curso)) return "Se requiere introducir un nombre de curso válido";
                if (!Validaciones.Entero(cantidad)) return "Se requiere introducir una cantidad de alumnos válida";
                if (fecha == "Seleccionar fecha") return "Se requiere introducir una fecha";

                using (var db = new ComputacionFCQContext())
                {
                    DateTime inicio = Fecha.ParseFecha(fecha);
                    DateTime fin = inicio.AddHours(hf);
                    inicio = inicio.AddHours(hi);

                    if (inicio == fin) return "La hora de inicio y fin deben ser distintas";
                    if (inicio > fin) return "La hora de inicio debe ser futura a la hora de fin";
                    if (inicio < DateTime.Now) return "La fecha de inicio no puede ser pasada";

                    //Buscando una reservacion que se empalme con la que se quiere agregar
                    Reservacion? reservacion = db.Reservacions.Where(x => x.SalaId == sala_id && x.Activa == true &&
                        ((x.FechaInicio >= inicio && x.FechaInicio < fin) || (x.FechaFin > inicio && x.FechaFin < fin) ||
                                (x.FechaInicio <= inicio && x.FechaFin >= fin))).FirstOrDefault();

                    if (reservacion != null)
                    {
                        if (reservacion.FrecuenciaId == null)
                            return $"La reservación se empalma con la reservación \"{reservacion.Curso}\" con fecha {reservacion.FechaInicio.Value.ToString("dd/MM/yyyy")}";
                        else
                            return $"La reservación se empalma con la reservación \"{reservacion.Frecuencia.Curso}\" " +
                                $"con fecha {reservacion.FechaInicio.Value.ToString("dd/MM/yyyy")}";
                    }

                    return null;
                }
            }
            catch { return "Error desconocido en Models.Reservacion.ValidarReservacion(unica), actualice la pagina y vuelva a intentarlo"; }
        }

        //Valida que la reservacion FRECUENCIAL pueda ser creada (retorna: null si es valida, string con mensaje de error si no)
        public static string? ValidarReservacion(string curso, string cantidad, int sala_id, string periodo_inicio, string periodo_fin, string[] dias)
        {
            try
            {
                if (!Validaciones.Curso(curso)) return "Se requiere introducir un nombre de curso válido";
                if (!Validaciones.Entero(cantidad)) return "Se requiere introducir una cantidad de alumnos válida";
                if (periodo_inicio == "Seleccionar fecha") return "Se requiere introducir una fecha de inicio de periodo";
                if (periodo_fin == "Seleccionar fecha") return "Se requiere introducir una fecha de fin de periodo";
                if (dias.Length == 0) return "Debe seleccionarse al menos un dia para crear la frecuencia";

                //dias va a llegar como un array de strings con formato: "{(int)DayOfWeek, hi, hf}" en orden ascendente conteniendo los dias seleccionados
                int[,] lista = new int[dias.Length, 3];

                //Desglozamos el string formateado en 3 columnas de ints 
                for (int i = 0; i < dias.Length; i++)
                {
                    lista[i, 0] = Convert.ToInt32(dias[i].Split('-')[0]); //DayOfWeek (Sunday=0, Monday=1...)
                    lista[i, 1] = Convert.ToInt32(dias[i].Split('-')[1]); //Hora inicio
                    lista[i, 2] = Convert.ToInt32(dias[i].Split('-')[2]); //Hora fin
                }

                using (var db = new ComputacionFCQContext())
                {
                    DateTime inicio = Fecha.ParseFecha(periodo_inicio);
                    DateTime fin = Fecha.ParseFecha(periodo_fin);
                    DateTime aux_inicio, aux_fin;

                    if (inicio == fin) return "La hora de inicio y fin deben ser distintas";
                    if (inicio > fin) return "La hora de inicio debe ser futura a la hora de fin";

                    //Con este metodo la cantidad de iteraciones (2 ifs 1 busqueda) seran = Cantidad Semanas * Cantidad de dias seleccionados
                    //Con la alternativa dia x dira la cantidad de iteraciones (2 ifs 1 busqueda) serrían 7 veces mas

                    //Avanzando semana por semana
                    while (inicio <= fin)
                    {
                        //Por cada dia seleccionado validamos que no interfiera
                        for (int i = 0; i < dias.Length; i++)
                        {
                            if (lista[i, 0] - (int)inicio.DayOfWeek >= 0)
                                aux_inicio = inicio.AddDays(lista[i, 0] - (int)inicio.DayOfWeek);
                            else
                                aux_inicio = inicio.AddDays(7 - (int)inicio.DayOfWeek + lista[i, 0]);

                            aux_fin = aux_inicio.AddHours(lista[i, 2]);
                            aux_inicio = aux_inicio.AddHours(lista[i, 1]);

                            if (aux_inicio > fin) break;
                            if (aux_inicio < DateTime.Now) return "No se pueden crear reservaciones en fechas pasadas";

                            //Buscando una reservacion que se empalme con la que se quiere agregar
                            Reservacion? reservacion = db.Reservacions.Where(x => x.SalaId == sala_id && x.Activa == true &&
                                ((x.FechaInicio >= aux_inicio && x.FechaInicio < aux_fin) || (x.FechaFin > aux_inicio && x.FechaFin < aux_fin) ||
                                        (x.FechaInicio <= aux_inicio && x.FechaFin >= aux_fin))).FirstOrDefault();

                            if (reservacion != null)
                            {
                                if (reservacion.FrecuenciaId == null)
                                    return $"La reservación se empalma con la reservación \"{reservacion.Curso}\" con fecha {reservacion.FechaInicio.Value.ToString("dd/MM/yyyy")}";
                                else
                                    return $"La reservación se empalma con la reservación \"{reservacion.Frecuencia.Curso}\" " +
                                        $"con fecha {reservacion.FechaInicio.Value.ToString("dd/MM/yyyy")}";
                            }
                        }
                        inicio = inicio.AddDays(7);
                    }
                }
                return null;
            }
            catch { return "Error desconocido en Models.Reservacion.ValidarReservacion(Frecuencial), actualice la pagina y vuelva a intentarlo"; }
        }

        public static string GetReservacionPorId(int id)
        {
            try
            {
                List<string> dias = new List<string> { "Lu", "Ma", "Mi", "Ju", "Vi", "Sa" };
                string json = "{";
                using (var db = new ComputacionFCQContext())
                {
                    var reservacion = db.Reservacions.Find(id);

                    json += $"matricula: {reservacion.Usuario.Matricula}, ";
                    json += $"cantidad_alumnos: {reservacion.CantidadAlumnos}, ";
                    json += $"sala_id: {reservacion.SalaId}, ";
                    json += $"programa: '{reservacion.Programa.Nombre}', ";
                    json += reservacion.Activa == true ? "activa: true, " : "activa: false, ";

                    DateTime dt = reservacion.FechaInicio.Value;
                    json += $"fecha: '{dt.Day}/{Fecha.GetMes(dt.Month)}/{dt.Year}', ";
                    json += $"hi: '{dt.Hour}', ";
                    json += $"hf: '{reservacion.FechaFin.Value.Hour}', ";

                    //Si es unica
                    if (reservacion.FrecuenciaId == null)
                    {
                        json += "es_unica: true, ";
                        json += $"curso: '{reservacion.Curso}', ";
                    }
                    //Si es frecuencial
                    else
                    {
                        if (db.Reservacions.Where(x => x.FrecuenciaId == reservacion.FrecuenciaId && x.Activa == true).Count() == 0)
                            json += "estado: 'Cancelada',";

                        json += $"totales: {db.Reservacions.Where(x => x.FrecuenciaId == reservacion.FrecuenciaId).Count()},";
                        json += $"restantes: {db.Reservacions.Where(x => x.FrecuenciaId == reservacion.FrecuenciaId && x.FechaInicio.Value > DateTime.Now).Count()},";

                        json += "es_unica: false, ";
                        json += $"curso: '{reservacion.Frecuencia.Curso}', ";

                        dt = reservacion.Frecuencia.PeriodoFin.Value;
                        json += $"periodo_fin: '{dt.Day}/{Fecha.GetMes(dt.Month)}/{dt.Year}', ";

                        dt = reservacion.Frecuencia.PeriodoInicio.Value;
                        json += $"periodo_inicio: '{dt.Day}/{Fecha.GetMes(dt.Month)}/{dt.Year}', ";

                        //Se envia arreglo de los dias que se presentan
                        DateTime dt_lim = dt.AddDays(7);
                        json += "dias: [";
                        while (dt.Date < dt_lim.Date)
                        {
                            //Se busca una reservacion de la misma frecuencia en el dia seleccionado
                            var aux = db.Reservacions.Where(x => x.FrecuenciaId == reservacion.FrecuenciaId && x.FechaInicio.Value.Date == dt.Date).FirstOrDefault();
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
            catch { return "{}"; }
        }

        //Retorna string en formato JSON con las reservaciones en la sala y semana indicada
        public static string GetReservacionesSemana(int sala, DateTime dt)
        {
            try
            {
                List<string> dias = new List<string> { "Lu", "Ma", "Mi", "Ju", "Vi", "Sa" };
                string json = "[";
                using (var db = new ComputacionFCQContext())
                {
                    var lista = db.Reservacions.Where(x => x.Activa == true && x.SalaId == sala && x.FechaInicio > dt && x.FechaInicio < dt.AddDays(6)).ToList();

                    foreach (Reservacion rsv in lista)
                    {
                        json += "{";
                        json += $"id: {rsv.Id},";
                        json += $"nombre: '{db.Usuarios.Find(rsv.UsuarioId).Nombre} {db.Usuarios.Find(rsv.UsuarioId).Apellidos}',";
                        json += rsv.Activa == true ? "activa: true, " : "activa: false, ";

                        if (rsv.FrecuenciaId == null)
                            json += $"curso: '{rsv.Curso}', backgroundColor: 'bisque',";
                        else
                            json += $"curso: '{rsv.Frecuencia.Curso}', backgroundColor: 'lavender',";

                        json += "targetIds: [";
                        for (int hora = rsv.FechaInicio.Value.Hour; hora < rsv.FechaFin.Value.Hour; hora++)
                            json += $"'{dias[((int)rsv.FechaInicio.Value.DayOfWeek) - 1] + hora}',";
                        json = json.Remove(json.Length - 1);

                        json += "]},";
                    }
                }
                return json + "]";
            }
            catch { return "[]"; }
        }
    }
}
