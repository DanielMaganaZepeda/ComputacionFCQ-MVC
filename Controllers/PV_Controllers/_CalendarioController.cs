using Microsoft.AspNetCore.Mvc;
using ComputacionFCQ_MVC.Models;

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
            return Json(Reservacion.GetReservacionesSemana(sala, Convert.ToDateTime(dt)));
        }
    }
}
