using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banha_UniverCity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public DepartmentController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var listOfDepartments = _unitOfWork.departmentRepository.Get(null, e => e.Courses);
            return View(listOfDepartments);
        }
        public IActionResult Details(int id)
        {
            var department = _unitOfWork.departmentRepository.getSpacifcDetails(id);//complex Qyery
            return View(department);
        }

        [HttpGet]
        public IActionResult UpSert(int? id)
        {
            Department department = new();
            if (id != null)
            {
                department = _unitOfWork.departmentRepository.GetOne(e => e.DepartmentID == id);
                return PartialView(department);
            }
            return PartialView(department);
        }
        [HttpPost]
        public async Task<IActionResult> UpSert(Department department ,IFormFile Cover)
        {
            if (ModelState.IsValid)
            {


                if (Cover != null)
                {
                    // Define the path where the file will be saved
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Covers", Cover.FileName);

                    // Check if the file with the same name already exists
                    if (System.IO.File.Exists(filePath))
                    {
                        ModelState.AddModelError("Cover", "A file with the same name already exists.");
                    }
                    else
                    {
                        // Generate a unique file name
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Cover.FileName;

                        // Define the new path where the file will be saved
                        var newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Covers", uniqueFileName);

                        // Save the file
                        using (var stream = new FileStream(newFilePath, FileMode.Create))
                        {
                            await Cover.CopyToAsync(stream);  // Use ImgCover instead of Content
                        }

                        // Assign the unique file name to the model property
                        department.Cover = uniqueFileName;
                    }
                }
                else
                {
                    ModelState.AddModelError("ImgCover", "Please upload an image.");
                }









                if (department.DepartmentID == 0)
                {
                    _unitOfWork.departmentRepository.Create(department);
                    TempData["alert"] = "Added successfully";


                }
                else
                {
                    _unitOfWork.departmentRepository.Edit(department);
                    TempData["alert"] = "Edited successfully";
                }
                _unitOfWork.Commit();
                var result = StaticData.CheckValidation(ModelState, Request, true);
            
                    return result;
                
            }
            else
            {
                var result = StaticData.CheckValidation(ModelState, Request, false);
               
                    return result;
                
              
            }
        }

        public IActionResult Delete(int id)
        {
            var department = _unitOfWork.departmentRepository.GetOne(e => e.DepartmentID == id);
            if (department != null)
            {
                _unitOfWork.departmentRepository.Delete(department);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }
            else { return NotFound(); }
        }

        public IActionResult Courses(int? id) 
        {
            var listOfCourses=_unitOfWork.courseRepository.Get(e=>e.DepartmentId == id,e=>e.Department);
            return View(listOfCourses);
        }
    }
}
