using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Banha_UniverCity.Areas.Student.Controllers
{
    [Area("Student")]
    public class StudentAssinmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManger;

        public StudentAssinmentController(UserManager<IdentityUser> userManger, IUnitOfWork unitOfWork)
        {
            _userManger = userManger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var submissions = _unitOfWork.assinmentSubmitionRepository.Get(e => e.ApplicationUserID == _userManger.GetUserId(User));
            if (submissions == null || !submissions.Any())
            {
                return NotFound("No submissions found for this assignment.");
            }
            return View(submissions);
        }
        [HttpGet]
        public IActionResult UploadSoluation(int id)
        {
            var assignment = new AssignmentSubmission()
            {
                AssignmentID = id,
            };

            return PartialView("_UploadSoluation", assignment);
        }

        [HttpPost]
        public IActionResult UploadSoluation(AssignmentSubmission model, string url)
        {
            JsonResult result;
            if(!ModelState.IsValid)
            {
                result = StaticData.CheckValidation(ModelState, Request, false);
                return result;
            }

            var userId = _userManger.GetUserId(User);
            if (userId == null)
            {
                ModelState.AddModelError("", "User not found.");
                result = StaticData.CheckValidation(ModelState, Request, false);
                return result;

            }
            model.ApplicationUserID = userId;
            model.SubmissionDate = DateTime.Now;
            // Check if the assignment already exists for the user
            var existingSubmission = _unitOfWork.assinmentSubmitionRepository
                .GetOne(e => e.AssignmentID == model.AssignmentID && e.ApplicationUserID == userId);
            if (existingSubmission != null)
            {
                ModelState.AddModelError("", "You have already submitted this assignment.");
                result = StaticData.CheckValidation(ModelState, Request, false);
                return result;

            }
            // Create the new submission
            _unitOfWork.assinmentSubmitionRepository.Create(model);
            _unitOfWork.Commit();
            result = StaticData.CheckValidation(ModelState, Request, true);
            return result;

        }
    }
}
