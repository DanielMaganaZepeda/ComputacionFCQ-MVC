using ComputacionFCQ_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputacionFCQ_MVC.Controllers
{
    public class ProgramasController : Controller
    {
        public IActionResult Programas()
        {
            return View(Programa.GetProgramas());
        }

        [HttpPost]
        public IActionResult ActualizarProgramas(string[] ids)
        {
            Programa.ActualizarProgramas(ids);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult AgregarPrograma(string nombre)
        {
            string? result = Programa.AgregarPrograma(nombre);
            if (result != null)
                return Json(new { success = false, responseText = result });
            else
                return Json(new { success = true });
        }

        [HttpDelete]
        public IActionResult EliminarPrograma(string id)
        {
            Programa.EliminarPrograma(id);
            return Json(new { success = true });
        }
    }
}
