using ComputacionFCQ_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace ComputacionFCQ_MVC.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult Reportes()
        {
            if (HttpContext.Session.GetString("usuario") == null)
            {
                TempData["direccion"] = "Reportes";
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        [HttpGet]
        public FileResult GenerarReporte(string tipo, string desde, string hasta)
        {
            return File(GeneracionReportes.GenerarReporte(tipo, desde, hasta).GetAsByteArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                tipo+"_"+DateTime.Now.ToString("dd-MM-yyyy")+".xlsx");
        }
    }
}
