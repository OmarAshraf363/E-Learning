using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models;
using BFCAI.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Banha_UniverCity.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    public class ExamController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public ExamController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            // الحصول على الامتحانات الخاصة بالمدرس الحالي
            var userId = _userManager.GetUserId(User);
            var examList = _unitOfWork.examRepository.Get(e => e.InstructorId == userId,e=>e.Course,expression=>expression.Questions);
            return View(examList);
        }

        public IActionResult Upsert(int? id)
        {
            Exam exam = new Exam();

            if (id == null)
            {
                return View(exam);
            }

            exam = _unitOfWork.examRepository.GetOne(e=>e.ExamID==id);
            ViewBag.courses=_unitOfWork.courseRepository.Get(e=>e.InstructorId==_userManager.GetUserId(User)).ToList();
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Exam exam)
        {
            if (ModelState.IsValid)
            {
                // ربط الامتحان بالمدرس الحالي
                exam.InstructorId = _userManager.GetUserId(User);

                if (exam.ExamID == 0)
                {
                    _unitOfWork.examRepository.Create(exam);
                }
                else
                {
                    _unitOfWork.examRepository.Edit(exam);
                }

                _unitOfWork.Commit();
                return RedirectToAction(nameof(Index));
            }

            return View(exam);
        }

        public IActionResult Questions(int? id)
        {
            var listOfQuestions=_unitOfWork.qusetionRepository.Get(e=>e.ExamID==id,e=>e.Choices,expression=>expression.Exam);
            ViewBag.ExamId=id;
            ViewBag.examName = _unitOfWork.examRepository.GetOne(e => e.ExamID == id)?.Title;
            return View(listOfQuestions);
        }
        public IActionResult UpSertQuestions(int? id, int examId,bool? outSide)
        {
            QuestionVM model = new();

            if (id.HasValue)
            {
               
                var question = _unitOfWork.qusetionRepository.GetOne(e => e.QuestionID == id);
                if (question == null) { return NotFound(); }

                model.QuestionID = question.QuestionID;
                model.QuestionText = question.QuestionText;
                model.Choices = _unitOfWork.choicesRepository.Get(e => e.QuestionID == id).ToList();
                model.ExamID = question.ExamID;
            }
            else
            {
              
                model.ExamID = examId ;
            }
            if (outSide==true)
            {
                return PartialView(model);
            }
            else
            {
            return View(model);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSertQuestions(QuestionVM model)
        {
            if (ModelState.IsValid)
            {
                Question question;

                if (model.QuestionID == 0)
                {

                    question = new Question
                    {
                        ExamID = model.ExamID,
                        QuestionText = model.QuestionText
                    };

                    _unitOfWork.qusetionRepository.Create(question);
                    _unitOfWork.Commit();


                    List<QuestionChoice> choices = new List<QuestionChoice>();
                    foreach (var choice in model.Choices)
                    {
                        choices.Add(new QuestionChoice()
                        {
                            QuestionID = question.QuestionID,
                            ChoiceText = choice.ChoiceText,
                            IsCorrect = choice.IsCorrect,
                        });
                    }

                    _unitOfWork.choicesRepository.AddRange(choices);
                    _unitOfWork.Commit();
                }
                else
                {

                    question = _unitOfWork.qusetionRepository.GetOne(e => e.QuestionID == model.QuestionID);
                    if (question == null) { return NotFound(); }

                    question.QuestionText = model.QuestionText;
                    question.ExamID = model.ExamID;

                    _unitOfWork.qusetionRepository.Edit(question);


                    var oldChoices = _unitOfWork.choicesRepository.Get(e => e.QuestionID == question.QuestionID).ToList();
                    _unitOfWork.choicesRepository.DeleteRange(oldChoices);
                    _unitOfWork.Commit();


                    List<QuestionChoice> newChoices = new List<QuestionChoice>();
                    foreach (var choice in model.Choices)
                    {
                        newChoices.Add(new QuestionChoice()
                        {
                            QuestionID = question.QuestionID,
                            ChoiceText = choice.ChoiceText,
                            IsCorrect = choice.IsCorrect,
                        });
                    }

                    _unitOfWork.choicesRepository.AddRange(newChoices);
                    _unitOfWork.Commit();
                }

                return RedirectToAction("Index");
            }


            ViewBag.ExamId = model.ExamID;
            return View(model);
        }



        //[HttpGet]
        ////public IActionResult UpSertListQuestions(int examId, bool? outSide)
        ////{
        ////    // إنشاء الـ ViewModel
        ////    QuestionsVM model = new();

        ////    // جلب الأسئلة المرتبطة بالامتحان من قاعدة البيانات
        ////    var questions = _unitOfWork.qusetionRepository.Get(e => e.ExamID == examId).ToList();

        ////    // تحويل الأسئلة إلى QuestionVM وإضافتها إلى نموذج الأسئلة
        ////    foreach (var question in questions)
        ////    {
        ////        model.Questions.Add(new QuestionVM
        ////        {
        ////            QuestionID = question.QuestionID,
        ////            QuestionText = question.QuestionText,
        ////            ExamID = question.ExamID,
        ////            Choices = _unitOfWork.choicesRepository.Get(c => c.QuestionID == question.QuestionID).ToList()
        ////        });
        ////    }

        ////    // إذا لم يكن هناك أسئلة بالفعل، يمكن إظهار حقل لإضافة سؤال جديد
        ////    if (!model.Questions.Any())
        ////    {
        ////        model.Questions.Add(new QuestionVM { ExamID = examId });
        ////    }

        ////    // إذا كان النموذج يجب عرضه في PartialView، يتم إعادته كذلك
        ////    if (outSide == true)
        ////    {
        ////        return PartialView(model);
        ////    }
        ////    else
        ////    {
        ////        return View(model);
        ////    }
        ////}















        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult UpSertQuestions(QuestionsVM model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (var questionVM in model.Questions)
        //        {
        //            Question question;

        //            if (questionVM.QuestionID == 0)
        //            {
        //                // إنشاء سؤال جديد
        //                question = new Question
        //                {
        //                    ExamID = questionVM.ExamID,
        //                    QuestionText = questionVM.QuestionText
        //                };

        //                _unitOfWork.qusetionRepository.Create(question);
        //                _unitOfWork.Commit();

        //                // إضافة الاختيارات المرتبطة بالسؤال
        //                List<QuestionChoice> choices = new List<QuestionChoice>();
        //                foreach (var choice in questionVM.Choices)
        //                {
        //                    choices.Add(new QuestionChoice()
        //                    {
        //                        QuestionID = question.QuestionID,
        //                        ChoiceText = choice.ChoiceText,
        //                        IsCorrect = choice.IsCorrect
        //                    });
        //                }

        //                _unitOfWork.choicesRepository.AddRange(choices);
        //                _unitOfWork.Commit();
        //            }
        //            else
        //            {
        //                // تعديل سؤال موجود
        //                question = _unitOfWork.qusetionRepository.GetOne(e => e.QuestionID == questionVM.QuestionID);
        //                if (question == null) { return NotFound(); }

        //                question.QuestionText = questionVM.QuestionText;
        //                question.ExamID = questionVM.ExamID;
        //                _unitOfWork.qusetionRepository.Edit(question);

        //                // تعديل الاختيارات
        //                var oldChoices = _unitOfWork.choicesRepository.Get(e => e.QuestionID == question.QuestionID).ToList();
        //                _unitOfWork.choicesRepository.DeleteRange(oldChoices);
        //                _unitOfWork.Commit();

        //                List<QuestionChoice> newChoices = new List<QuestionChoice>();
        //                foreach (var choice in questionVM.Choices)
        //                {
        //                    newChoices.Add(new QuestionChoice()
        //                    {
        //                        QuestionID = question.QuestionID,
        //                        ChoiceText = choice.ChoiceText,
        //                        IsCorrect = choice.IsCorrect
        //                    });
        //                }

        //                _unitOfWork.choicesRepository.AddRange(newChoices);
        //                _unitOfWork.Commit();
        //            }
        //        }

        //        return RedirectToAction("Index");
        //    }

        //    return View(model);
        //}






    }
}
