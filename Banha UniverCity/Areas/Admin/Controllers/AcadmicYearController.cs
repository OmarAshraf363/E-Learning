using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banha_UniverCity.Areas.Admin.Controllers
{
    [Area("Admin")]
   
    public class AcadmicYearController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public AcadmicYearController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var listOfYears = _unitOfWork.academicYear.Get();
            return View(listOfYears);
        }

        // Upsert Action to Create or Edit an academic year
        public IActionResult Upsert(int? id)
        {
            AcademicYear year = new AcademicYear();
            if (id == null || id == 0)
            {
                // Create new academic year
                return PartialView(year);
            }
            else
            {
                // Edit existing academic year
                year = _unitOfWork.academicYear.GetOne(u => u.AcademicYearID == id);
                if (year == null)
                {
                    return NotFound();
                }
                return PartialView(year);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(AcademicYear year)
        {
            if (ModelState.IsValid)
            {
                if (year.AcademicYearID == 0)
                {
                    _unitOfWork.academicYear.Create(year); // Create
                }
                else
                {
                    _unitOfWork.academicYear.Edit(year); // Update
                }
                var result=StaticData.CheckValidation(ModelState,Request,true);
                if(result!=null) { return result; }
                _unitOfWork.Commit();
                return RedirectToAction(nameof(Index));
            }
            else
            {

            var result = StaticData.CheckValidation(ModelState, Request, false);
            if (result != null) { return result; }
                return BadRequest();

            }
        }

        // Delete Action
       
        public IActionResult Delete(int id)
        {
            var year = _unitOfWork.academicYear.GetOne(u => u.AcademicYearID == id);
            if (year == null)
            {
                return NotFound();
            }
            _unitOfWork.academicYear.Delete(year);
            _unitOfWork.Commit();
            return RedirectToAction(nameof(Index));
        }
    }
}
