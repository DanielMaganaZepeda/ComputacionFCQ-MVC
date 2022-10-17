using Microsoft.AspNetCore.Mvc;

namespace ComputacionFCQ_MVC.Controllers
{
    public class ReservacionesController : Controller
    {
        public IActionResult Reservaciones()
        {
            return View();
        }

    }
}
