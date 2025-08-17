using Banha_UniverCity.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Banha_UniverCity.Areas.Instructor.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetQuestions(int id)
        {
            var listOfQuestions = _unitOfWork.qusetionRepository
                .Get(e => e.ExamID == id, e => e.Choices);
            ViewBag.ExamId = id;
            return PartialView("Questions", listOfQuestions);
        }
    }
}
