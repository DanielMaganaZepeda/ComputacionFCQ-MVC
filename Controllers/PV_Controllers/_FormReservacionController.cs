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
            string curso, string programa, string cantidad, int sala_id, string fecha, int hi, int hf)
        {
            string? result = Usuario.ValidarDatos(matricula, nombre, apellidos, correo);
            if (result != null)
                return Json(new { success = false, responseText = result });

            result = Reservacion.ValidarReservacion(curso, cantidad, sala_id, fecha, hi, hf);
            if (result != null)
                return Json(new { success = false, responseText = result});

            Usuario.GuardarCambios(matricula, nombre, apellidos, correo, carrera, false);
            Reservacion.AgregarReservacion(matricula, curso, cantidad, sala_id, programa, fecha, hi, hf);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult AgregarReservacionFrecuencial(string matricula, string nombre, string apellidos, string correo, string carrera,
            string curso, string programa, string cantidad, int sala_id, string periodo_inicio, string periodo_fin, string[] dias)
        {
            string? result = Usuario.ValidarDatos(matricula, nombre, apellidos, correo);
            if (result != null)
                return Json(new { success = false, responseText = result });

            result = Reservacion.ValidarReservacion(curso, cantidad, sala_id, periodo_inicio, periodo_fin, dias);
            if (result != null)
                return Json(new { success = false, responseText = result });

            int total = Reservacion.AgregarReservacion(matricula, curso, cantidad, sala_id, programa, periodo_inicio, periodo_fin, dias);
            return Json(new { success = true , total = total });
        }

        [HttpDelete]
        public IActionResult CancelarEvento(string id)
        {
            Reservacion.CancelarEvento(id);
            return Json(new {success = true });
        }

        [HttpDelete]
        public IActionResult CancelarFrecuencia(int id)
        {
            Reservacion.CancelarFrecuencia(id);
            return Json(new { success = true });
        }

    }
}
