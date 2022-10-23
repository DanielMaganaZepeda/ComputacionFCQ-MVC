using Microsoft.AspNetCore.Mvc;
using ComputacionFCQ_MVC.Models;

namespace ComputacionFCQ_MVC.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            if (TempData["direccion"] == null)
                TempData["direccion"] = "Sesiones";

            return View(TempData["direccion"]);
        }

        public IActionResult Acceder(string usuario, string contrasena)
        {
            string? admin = Administrador.ValidarUsuario(usuario, contrasena);

            if (admin != null)
            {
                HttpContext.Session.SetString("usuario", admin);
                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }
    }
}
