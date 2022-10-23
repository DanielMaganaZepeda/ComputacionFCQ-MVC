using Microsoft.AspNetCore.Mvc;
using ComputacionFCQ_MVC.Models;
using Newtonsoft.Json.Linq;

namespace ComputacionFCQ_MVC.Controllers
{
    public class ReservacionesController : Controller
    {
        public IActionResult Reservaciones()
        {
            if (HttpContext.Session.GetString("usuario") == null)
            {
                TempData["direccion"] = "Reservaciones";
                return RedirectToAction("Login", "Login");
            }
            return View(); 
        }

        [HttpGet]
        public IActionResult ReservacionDetalle(int id)
        {
            var jObject = JObject.Parse(Reservacion.GetReservacionPorId(id));
            return Content(jObject.ToString(), "application/json");
        }

        [HttpPost]
        public IActionResult AplicarFiltros(string desde, string hasta, int[] salas, string[] tipos, string?[] estados)
        {
            var jObject = JArray.Parse(Reservacion.GetReservacionesFiltradas(desde, hasta, salas, tipos, estados));
            return Content(jObject.ToString(), "application/json");
        }

        [HttpGet]        
        public IActionResult GetFechas()
        {
            return Json(new { inicio = Fecha.ParseFecha(Fecha.GetInicio(null).Fecha1.Value), fin = Fecha.ParseFecha(Fecha.GetFinal(null).Fecha1.Value) });
        }

        [HttpPost]
        public IActionResult ActualizarFechas(string inicio, string fin)
        {
            string? result = Fecha.ActualizarFechas(inicio, fin);

            if (result == null)
                return Json(new { success = true });
            else
                return Json(new { success = false, responseText = result });
        }
    }
}
