using Microsoft.AspNetCore.Mvc;
using ComputacionFCQ_MVC.Models;

namespace ComputacionFCQ_MVC.Controllers.PV_Controllers
{
    public class _FormReservacionController : Controller
    {
        public IActionResult _FormReservacionPartial()
        {
            return PartialView("_FormReservacion");
        }

        [HttpGet]
        public IActionResult GetProgramas(int sala)
        {
            return Json(Sala.GetProgramasPorSala(sala));
        }

        [HttpPost]
        public IActionResult AgregarReservacionUnica(string matricula, string nombre, string apellidos, string correo, string carrera,
            string curso, string cantidad, int sala_id, string fecha, int hi, int hf)
        {
            string? result = Usuario.ValidarDatos(matricula, nombre, apellidos, correo);
            if (result != null)
                return Json(new { success = false, responseText = result });

            result = Reservacion.ValidarReservacion(curso, cantidad, sala_id, fecha, hi, hf);
            if (result == null)
                return Json(new { success = true });

            Usuario.GuardarCambios(matricula, nombre, apellidos, correo, carrera, false);
            //Llamada a agregar reservacion
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult AgregarReservacionFrecuencial(string matricula, string nombre, string apellidos, string correo, string carrera,
            string curso, string cantidad, int sala_id, string periodo_inicio, string periodo_fin, string[] dias)
        {
            string? result = Usuario.ValidarDatos(matricula, nombre, apellidos, correo);
            if (result != null)
                return Json(new { success = false, responseText = result });

            result = Reservacion.ValidarReservacion(curso, cantidad, sala_id, periodo_inicio, periodo_fin, dias);
            if (result != null)
                return Json(new { success = false, responseText = result });

            return Json(new { success = true });
        }
    }
}
