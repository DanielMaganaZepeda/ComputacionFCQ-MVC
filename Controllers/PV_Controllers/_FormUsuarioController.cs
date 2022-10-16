using ComputacionFCQ_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComputacionFCQ_MVC.Controllers.PV_Controllers
{
    public class _FormUsuarioController : Controller
    {
        //Recibe como parametro NULL o la matricula si se quieren los detalles
        public IActionResult _FormUsuarioPartial(string matricula)
        {
            if(matricula == null)
            {
                Usuario usuario = new Usuario();
                return PartialView("_FormUsuario", usuario);
            }
            else
            {
                return PartialView("_FormUsuario", Usuario.GetUsuarioPorMatricula(matricula));
            }
        }

        //Busca el usuario, si lo encuentra lo devuelve como json, si no lo encuentra devuelve un mensaje de error
        [HttpGet]
        public IActionResult BuscarUsuario(string matricula)
        {
            if (!Validaciones.Entero(matricula))
                return Json(new { responseText = "Debe introducir una matricula valida" });

            Usuario? usuario = Usuario.GetUsuarioPorMatricula(matricula);

            if (usuario != null)
                return Json(new { matricula=usuario.Matricula, nombre=usuario.Nombre, apellidos=usuario.Apellidos, correo=usuario.Correo,
                                  carrera_id=usuario.CarreraId, es_alumno=usuario.EsAlumno });
            else
                return Json(new { responseText = "No se encontraron datos del usuario con la matricula introducida" });
        }
    }
}
