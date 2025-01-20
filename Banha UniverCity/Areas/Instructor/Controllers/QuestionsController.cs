using Microsoft.AspNetCore.Mvc;

namespace Banha_UniverCity.Areas.Instructor.Controllers
{
    public class QuestionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
