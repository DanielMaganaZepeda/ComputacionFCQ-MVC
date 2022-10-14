using ComputacionFCQ_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputacionFCQ_MVC.Controllers.PV_Controllers
{
    public class _FormUsuarioController : Controller
    {
        public IActionResult _FormUsuarioPartial(Usuario usuario)
        {
            return PartialView("_FormUsuario",usuario);
        }
    }
}
