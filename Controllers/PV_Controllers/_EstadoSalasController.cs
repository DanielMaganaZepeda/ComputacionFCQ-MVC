using ComputacionFCQ_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputacionFCQ_MVC.Controllers.PV_Controllers
{
    public class _EstadoSalasController : Controller
    {
        public IActionResult EstadoSalasPartial()
        {
            //Lista trae los mensajes que se usaran para ponerse en la tabla
            List<string> lista = new List<string>();
            using (var db = new ComputacionFCQContext())
            {
                //Se obtienen las IDS de las sals
                List<int> salas = db.Salas.Select(x => x.Id).ToList();

                foreach (int sala_id in salas)
                {
                    //Obtenermos la lista de reservaciones activas que haya hoy en cada sala, que esten en curso o que empiecen mas tarde
                    List<Reservacion> reservaciones = db.Reservacions.Where(x => x.FechaInicio.Value.DayOfYear == DateTime.Now.DayOfYear && x.Activa == true && x.SalaId == sala_id &&
                    ((x.FechaInicio.Value <= DateTime.Now && x.FechaFin.Value > DateTime.Now) || (x.FechaInicio.Value > DateTime.Now))).OrderBy(x=>x.FechaInicio).ToList();

                    //Si no hay reservaciones ese dia
                    if (reservaciones.Count == 0)
                    {
                        lista.Add($"{sala_id}-Verde-Disponible por lo que resta del día");
                    }
                    else
                    {
                        //Si esta en transcurso
                        if (reservaciones[0].FechaInicio.Value <= DateTime.Now)
                        {
                            int i = 0, j = 0;
                            //Mientras haya reservaciones que empiecen cuando termina la siguiente
                            while (reservaciones.Count >= j + 2)
                            {
                                if (reservaciones[i].FechaFin.Value == reservaciones[i + 1].FechaInicio.Value)
                                {
                                    i++;
                                }
                                j++;
                            }
                            lista.Add($"{sala_id}-Rojo-Ocupada hasta las {reservaciones[i].FechaFin.Value.ToString("HH:mm")}");
                        }
                        else
                        {
                            //Si empieza en menos de una hora
                            if (reservaciones[0].FechaInicio.Value - DateTime.Now < new TimeSpan(1, 0, 0))
                            {
                                lista.Add($"{sala_id}-Amarillo-Disponible hasta las {reservaciones[0].FechaInicio.Value.ToString("HH:mm")}");
                            }
                            //Si empieza en mas de una hora
                            else
                            {
                                lista.Add($"{sala_id}-Verde-Disponible hasta las {reservaciones[0].FechaInicio.Value.ToString("HH:mm")}");
                            }
                        }
                    }
                }
            }
            return PartialView("_EstadoSalas",lista);
        }
    }
}
