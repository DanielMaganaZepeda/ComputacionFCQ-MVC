using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComputacionFCQ_MVC.Models;
using System.Timers;
using ComputacionFCQ_MVC.Controllers.PV_Controllers;

namespace ComputacionFCQ_MVC.Controllers
{
    public class SesionesController : Controller
    {
        private readonly ComputacionFCQContext _context;

        public SesionesController(ComputacionFCQContext context)
        {
            _context = context;
        }

        public class DatosSesion
        {
            public DatosSesion(string matricula, int sala, int computadora,DateTime fecha_inicio)
            {
                this.matricula = matricula;
                this.sala = sala;
                this.computadora = computadora;
                this.fecha_inicio = fecha_inicio;
            }

            public string matricula { set; get; }
            public int sala { get; set; }
            public int computadora { get; set; }
            public DateTime fecha_inicio { get; set; }
        }

        public IActionResult TablaSesionesPartial()
        {
            List<DatosSesion> lista = new List<DatosSesion>();
            
            using (var db = new ComputacionFCQContext())
            {
                List<Sesion> sesiones = db.Sesions.Where(x => x.FechaFin.Value == null).ToList();

                foreach (Sesion sesion in sesiones)
                {
                    lista.Add(new DatosSesion
                        (db.Usuarios.Find(sesion.UsuarioId).Matricula,
                        db.Computadoras.Find(sesion.ComputadoraId).SalaId,
                        db.Computadoras.Find(sesion.ComputadoraId).Numero,
                        sesion.FechaInicio.Value));
                }
            }

            return PartialView("_TablaSesiones", lista);
        }

        [HttpPost]
        public IActionResult FinalizarSesion(string id)
        {
            if (id == null) return Json(new { success = false, responseText = "Debe introducir una matricula valida" });
            foreach (char c in id)
                if (!char.IsDigit(c)) return Json(new { success = false, responseText = "Debe introducir una matricula valida" });

            using (var db = new ComputacionFCQContext())
            {
                if (db.Sesions.Where(x => x.FechaFin.Value == null && x.UsuarioId == db.Usuarios.Where(y => y.Matricula == id).First().Id).FirstOrDefault()!=null)
                {
                    db.Sesions.Where(x => x.FechaFin.Value == null && x.UsuarioId == db.Usuarios.Where(y => y.Matricula == id).First().Id).First().FechaFin = DateTime.Now;
                    db.SaveChanges();
                    return Json(new { success = true, responseText = $"Se ha finalizado la sesion del usuario con la matricula {id}" });
                }
                else
                {
                    return Json(new { success = false, responseText = "El usuario no se encuentra en una sesion activa" });
                }
            }
        }

        public IActionResult Sesiones()
        {
            return View("~/Views/Sesiones/Sesiones.cshtml");
        }
    }
}
