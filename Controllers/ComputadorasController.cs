using Microsoft.AspNetCore.Mvc;
using ComputacionFCQ_MVC.Models;
using Newtonsoft.Json.Linq;

namespace ComputacionFCQ_MVC.Controllers
{
    public class ComputadorasController : Controller
    {
        public IActionResult Computadoras()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetReportes()
        {
            var jObject = JArray.Parse(Computadora.GetReportes());
            return Content(jObject.ToString(), "application/json");
        }

        [HttpPost]
        public IActionResult SolucionarReporte(string reporte_id, int sala_id, int numero)
        {
            int result = Computadora.SolucionarReporte(reporte_id, sala_id, numero);
            return Json(new { cantidad = result });
        }

        [HttpPost]
        public IActionResult AgregarReporte(string sala_id, string numero, string detalle)
        {
            string? result = Computadora.AgregarReporte(sala_id, numero, detalle);

            if (result == null)
                return Json(new { success = true });
            else
                return Json(new { success = false, responseText = result });
        }

        [HttpGet]
        public IActionResult ActualizarComputadoras(int sala_id)
        {
            string? result = Computadora.GetComputadoras(sala_id);
            var jObject = JArray.Parse(result);

            return Content(jObject.ToString(), "application/json");
        }

        [HttpGet]
        public IActionResult ComputadoraDetalle(int sala_id, int numero)
        {
            var jObject = JArray.Parse(Computadora.GetComputadoraDetalle(sala_id, numero));
            return Content(jObject.ToString(), "application/json");
        }
    }
}
