using Microsoft.AspNetCore.Mvc;
using ComputacionFCQ_MVC.Models;
using Newtonsoft.Json.Linq;

namespace ComputacionFCQ_MVC.Controllers.PV_Controllers
{
    public class _CalendarioController : Controller
    {
        public IActionResult _CalendarioPartial()
        {
            return PartialView("_Calendario");
        }

        [HttpGet]
        public IActionResult GetReservaciones(int sala, string dt)
        {
            var jObject = JArray.Parse(Reservacion.GetReservacionesSemana(sala, Convert.ToDateTime(dt)));
            return Content(jObject.ToString(), "application/json");
        }
    }
}
