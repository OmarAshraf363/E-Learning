using Microsoft.AspNetCore.Mvc;

namespace Banha_UniverCity.Areas.Student.Controllers
{
    [Area("Student")]
    public class StudentAssinmentController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
