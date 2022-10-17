using Microsoft.AspNetCore.Mvc;

namespace ComputacionFCQ_MVC.Controllers.PV_Controllers
{
    public class _DatePickerController : Controller
    {
        public IActionResult _DatePickerPartial()
        {
            return PartialView("_DatePicker");
        }
    }
}
