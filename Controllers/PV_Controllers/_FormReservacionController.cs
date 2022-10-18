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
    }
}
