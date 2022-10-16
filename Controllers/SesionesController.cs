using Microsoft.AspNetCore.Mvc;
using ComputacionFCQ_MVC.Models;

namespace ComputacionFCQ_MVC.Controllers
{
    public class SesionesController : Controller
    {
        private readonly ComputacionFCQContext _context;

        public SesionesController(ComputacionFCQContext context)
        {
            _context = context;
        }

        public IActionResult TablaSesionesPartial()
        {
            return PartialView("_TablaSesiones", Sesion.GetSesiones());
        }

        [HttpPost]
        public IActionResult IniciarSesion(string matricula, string nombre, string apellidos, string correo, string carrera, bool es_alumno, int sala, int computadora, string programa)
        {
            //Validamos los datos
            string? response = Usuario.ValidarDatos(matricula, nombre, apellidos, correo);
            if (response != null) return Json(new { success = false, responseText = response });

            //Validamos que no este en una sesion activa
            if (Usuario.EstaEnSesion(matricula)) return Json(new { success = false, responseText="El usuario ya se encuentra en una sesion activa"});

            //Actualizamos los datos
            Usuario.GuardarCambios(matricula, nombre, apellidos, correo, carrera, es_alumno);

            //Abrimos sesion
            Sesion.IniciarSesion(matricula, sala, computadora, programa);
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult ActualizarSalas()
        {
            return Json(Sala.GetSalasDisponibles());
        }

        [HttpGet]
        public IActionResult ActualizarComputadoras(int sala)
        {
            return Json(Sala.GetComputadorasPorSala(sala));
        }

        [HttpGet]
        public IActionResult ActualizarProgramas(int sala)
        {
            return Json(Sala.GetProgramasPorSala(sala));
        }

        [HttpPost]
        public IActionResult FinalizarSesion(string id)
        {
            string? response = Sesion.FinalizarSesion(id);

            if (response == null)
                return Json(new { success = true });
            else
                return Json(new { success = false, responseText = response });
        }

        public IActionResult Sesiones()
        {
            return View("~/Views/Sesiones/Sesiones.cshtml");
        }
    }
}
