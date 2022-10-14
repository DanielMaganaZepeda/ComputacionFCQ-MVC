using ComputacionFCQ_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputacionFCQ_MVC.Controllers.PV_Controllers
{
    public class _FormUsuarioController : Controller
    {
        public IActionResult _FormUsuarioPartial()
        {    
            return PartialView("_FormUsuario", null);
        }

        public IActionResult _FormUsuarioDetallePartial(Usuario usuario)
        {
            return PartialView("_FormUsuario", usuario);
        }

        [HttpGet]
        public IActionResult BuscarUsuario(string matricula)
        {
            if(!Models.Usuario.ValidarMatricula(matricula))
                return Json(new { responseText = "Debe introducir una matricula valida" });   

            Usuario? usuario = Models.Usuario.GetUsuarioPorMatricula(matricula);
            if(usuario!=null)
                return Json(usuario);
            else
                return Json(new { responseText = "No se encontraron datos del usuario con la matricula introducida" });
        }
    }
}
