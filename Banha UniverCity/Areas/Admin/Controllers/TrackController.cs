using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Banha_UniverCity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TrackController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public TrackController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var tracks = _unitOfWork.trackRepository.Get(null,e=>e.Courses);
            return View(tracks);
        }
        public IActionResult Details(int id) 
        { 
            var track=_unitOfWork.trackRepository.GetOne(e=>e.Id==id,e=>e.Courses);
            return View("~/Views/Shared/Details.cshtml", track);
        }
        [HttpGet]
        public IActionResult UpSert(int? id) 
        {
            Track? track=new();
            if (id.HasValue)
            {
                 track=_unitOfWork.trackRepository.GetOne(e=>e.Id==id,e=>e.Courses);
                return View(track);
            }
            return View(track);
        }
        [HttpPost]
        public async Task <IActionResult> UpSert(Track track,IFormFile Cover)
        {
            JsonResult result;
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
                        track.Cover = uniqueFileName;
                    }
                }
                else
                {
                    ModelState.AddModelError("ImgCover", "Please upload an image.");
                }
                if (track.Id==0)
                {
                    _unitOfWork.trackRepository.Create(track);
                    _unitOfWork.Commit();
                }
                else
                {
                    _unitOfWork.trackRepository.Edit(track);
                    _unitOfWork.Commit();
                }
                result=StaticData.CheckValidation(ModelState,Request,true);
                return result;
            }
            else
            {
                result=StaticData.CheckValidation(ModelState, Request,false);
                return result;
            }
        }

      
        public IActionResult Delete(int id) 
        {
            var track=_unitOfWork.trackRepository.GetOne(e=>e.Id == id);
            if (track == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.trackRepository.Delete(track);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }
        }

    }
}
