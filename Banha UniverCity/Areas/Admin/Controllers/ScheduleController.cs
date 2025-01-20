using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Banha_UniverCity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Banha_UniverCity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ScheduleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public ScheduleController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var schedules = _unitOfWork.classSchedulere.Get(null,e=>e.Course,e=>e.Instructor,e=>e.Room);
            return View(schedules);
        }

        public IActionResult UpSert(int? id)
        {
            
            ScheduleViewModel model = new ScheduleViewModel
            {
                Courses = _unitOfWork.courseRepository.Get().Select(c => new SelectListItem
                {
                    Text = c.CourseName,
                    Value = c.CourseID.ToString()
                }),
                Instructors = _userManager.Users.Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                }),
                Rooms = _unitOfWork.roomRepository.Get().Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.RoomID.ToString()
                }),
                Departments = _unitOfWork.departmentRepository.Get().Select(d => new SelectListItem
                {
                    Text = d.DepartmentName,
                    Value = d.DepartmentID.ToString()
                }),
                Years = _unitOfWork.academicYear.Get().Select(y => new SelectListItem
                {
                    Text = y.YearName,
                    Value = y.AcademicYearID.ToString()
                })
            };

            if (id != null && id != 0)
            {
                var schedule = _unitOfWork.classSchedulere.GetOne(e => e.ClassScheduleId == id);

                if (schedule == null)
                {
                    return NotFound();
                }

                model.ClassScheduleId = schedule.ClassScheduleId;
                model.StartTime = schedule.StartTime;
                model.EndTime = schedule.EndTime;
                model.DayOfWeek = schedule.DayOfWeek;
                model.CourseID = schedule.CourseId;
                model.InstructorID = schedule.InstructorId != null ? int.Parse(schedule.InstructorId) : 0;
                model.RoomID = schedule.RoomId ?? 0;
                model.DepartmentId = schedule.DepartmentId;
                model.AcadmicYearId = schedule.AcadmicYearId;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(ScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                ClassSchedule schedule = model.ClassScheduleId == 0
                    ? new ClassSchedule()
                    : _unitOfWork.classSchedulere.GetOne(e => e.ClassScheduleId == model.ClassScheduleId);

                if (schedule == null)
                {
                    return NotFound();
                }

                schedule.StartTime = model.StartTime;
                schedule.EndTime = model.EndTime;
                schedule.DayOfWeek = model.DayOfWeek;
                schedule.CourseId = model.CourseID;
                schedule.InstructorId = model.InstructorID != 0 ? model.InstructorID.ToString() : null;
                schedule.RoomId = model.RoomID != 0 ? model.RoomID : null;
                schedule.DepartmentId = model.DepartmentId;
                schedule.AcadmicYearId = model.AcadmicYearId;

                if (model.ClassScheduleId == 0)
                {
                    _unitOfWork.classSchedulere.Create(schedule);
                }
                else
                {
                    _unitOfWork.classSchedulere.Edit(schedule);
                }

                _unitOfWork.Commit();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        public IActionResult Delete(int id)
        {
            var schedual = _unitOfWork.classSchedulere.GetOne(e => e.ClassScheduleId == id);
            if (schedual != null)
            {
                _unitOfWork.classSchedulere.Delete(schedual);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }
            else { return NotFound(); }
        }

    }
}
