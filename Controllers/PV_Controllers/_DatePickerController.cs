using Microsoft.AspNetCore.Mvc;
using ComputacionFCQ_MVC.Models;

namespace ComputacionFCQ_MVC.Controllers.PV_Controllers
{
    public class _DatePickerController : Controller
    {
        public IActionResult _DatePickerPartial(string id,string tipo)
        {
            if(tipo=="desde")
                return PartialView("_DatePicker",Fecha.GetInicio(id));
            else
                return PartialView("_DatePicker", Fecha.GetFinal(id));
        }
    }
}
