using Microsoft.AspNetCore.Mvc;
using ComputacionFCQ_MVC.Models;
using Newtonsoft.Json.Linq;

namespace ComputacionFCQ_MVC.Controllers
{
    public class ReservacionesController : Controller
    {
        public IActionResult Reservaciones()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ReservacionDetalle(int id)
        {
            var jObject = JObject.Parse(Reservacion.GetReservacionPorId(id));
            return Content(jObject.ToString(), "application/json");
        }
    }
}
