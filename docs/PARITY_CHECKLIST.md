# Parity Checklist: الـ MVC القديم مقابل الـ API و Angular

مراجعة لكل Controller Action وكل View في مشروع `Banha UniverCity`، وفين بقى مكانه في النسخة الجديدة.

**معاني العلامات:**
- ✅ اتنقل وشغّال.
- ⚠️ اتنقل مع إصلاح أو تغيير بسيط (التفاصيل في `MIGRATION_PLAN.md`).
- ➖ متنقلش عن قصد، والسبب مكتوب جنبه.

**طريقة التحقق:**
- 65 Integration test للـ API.
- 7 سيناريوهات E2E في `e2e/`، فيها 72 خطوة، وكلها نجحت على الكود النهائي.

---

## Admin
| Controller.Action | النسخة الجديدة | الحالة |
|---|---|---|
| DashBoard.Index | `GET /api/admin/dashboard` ← `/admin` | ⚠️ الإجمالي الحقيقي للكورسات، والكورس الأكثر شعبية |
| User.Index / Upsert / LockUser / UnlockUser / Delete | `/api/admin/users` ← `/admin/users`، `/admin/users/new`، `/admin/users/:id` | ⚠️ `UserType` بيتظبط، و Lock/Delete بقوا POST/DELETE |
| Department.Index / Details / UpSert / Delete / Courses | `/api/admin/departments` ← `/admin/departments`، `/:id`، `/:id/courses` + Modal | ⚠️ الغلاف بيفضل في التعديل، و 409 لو فيه داتا مرتبطة |
| Track.Index / Details / UpSert / Delete | `/api/admin/tracks` ← `/admin/tracks`، `/admin/tracks/:id` + Modal | ⚠️ زرار "New Track" اتضاف |
| AcadmicYear.Index / Upsert / Delete | `/api/admin/academic-years` ← `/admin/academic-years` | ⚠️ الحفظ من الـ Modal بقى شغّال |
| Course.Index / Details / UpSert / Delete | `/api/admin/courses` ← `/admin/courses`، `/new`، `/:id`، `/:id/details` | ⚠️ قوايم الـ Keywords والـ Objectives والـ Topics بقت بتتحفظ، والغلاف بيفضل |
| Schedule.Index / UpSert / Delete | `/api/admin/schedules` ← `/admin/schedules`، `/new`، `/:id` | ⚠️ الـ Instructor بقى بيتحفظ صح |
| Enrollment.Index / EnrollByEmail / UploadCsv | `/api/admin/enrollments` ← `/admin/enrollments` | ⚠️ الـ CSV بيتخطّى السطور الغلط، و "Enroll Manually" اتشال لأنه كان فاضي |
| Home.Index / Privacy (Admin) | — | ➖ صفحات Template فاضية |

## Customer (الواجهة العامة)
| Controller.Action | النسخة الجديدة | الحالة |
|---|---|---|
| Home.Index | `GET /api/catalog/home` ← `/` | ⚠️ الكاش اتصلّح، والأدمن والـ Instructor بيتحوّلوا لصفحاتهم زي الأصل |
| Home.GetByDepTId | `GET /api/catalog/departments/courses` ← تابات الـ Home | ✅ |
| Home.Courses + _partialFilterView | `GET /api/catalog/courses` ← `/courses?...` | ⚠️ الفلاتر بقت بتتطبّق فعلًا |
| Home.Details + GetObjectives + GetTopics | `GET /api/catalog/courses/{id}` ← `/courses/:id` | ⚠️ قايمة الترتيب و Load more اتشالوا لأنهم كانوا بينادوا Functions مش موجودة |
| Home.Category | `GET /api/catalog/categories(/{id})` ← `/category`، `/category/:id` | ✅ |
| Home.TopicCourse | `GET /api/catalog/topics/{word}` ← `/topic/:word` | ✅ |
| Home.Filter | ← `/topic/:word` | ➖ مكانش ليه View، والـ TopicCourse بيعمل نفس الشغل |
| Home.TrackDetails | `GET /api/catalog/tracks/{id}` ← `/tracks/:id` | ⚠️ الـ Progress بقى بيتحسب |
| Home.GetRegister / PostRegister | `POST /api/auth/register-instructor` ← Modal "Teach on BFCAI" | ⚠️ `UserType` بيتظبط |
| Home.Privacy / Error | الـ Errors بتظهر كـ SweetAlert، والرابط المجهول بيروح للـ Home | ➖ Privacy صفحة Template فاضية |
| Cart.Index / AddToCart / RemoveFromCart / Pay | `/api/cart` ← `/cart`، `/cart?add=`، زرار Checkout | ⚠️ السعر والكمية اتصلّحوا، وحذف عنصر بقى لصاحبه بس |
| checkout.Success / cancel | `POST /api/cart/checkout/confirm` ← `/cart/checkout/success`، `/cancel` | ⚠️ الدفع بيتأكد من Stripe قبل التسجيل |
| Community.Index | `GET /api/community/feed` ← `/community?...` | ✅ |
| Community.UpsertPost / DeletePost | `/api/community/posts` ← Modal + قايمة ☰ | ⚠️ لصاحب البوست بس، والحذف بقى شغّال، والـ Keywords والصورة اتصلّحوا |
| Community.UpsertComment / AddReply / GetComments / GetReplies | `/api/community/posts/{id}/comments`، `/comments/{id}/replies` | ⚠️ الكاتب بقى بيتاخد من التوكن |
| Community.AddReaction / addReactInComment / CountReaction / CountReactionInComment / GetReaction / GetCommentReaction | `/api/community/.../reactions` | ✅ |

## Instructor
| Controller.Action | النسخة الجديدة | الحالة |
|---|---|---|
| Instructor.Dashboard | `GET /api/instructor/dashboard` ← `/instructor` | ✅ |
| InstructorCourse.Index / UpSert، و Instructor.Courses (نسخة أقدم) | `/api/instructor/courses` ← `/instructor/courses`، `/new`، `/:id` | ⚠️ الملكية، وزرار Edit اتضاف |
| Instructor.UpsertCourseCurriculum | `POST /courses/{id}/sections`، `PUT /sections/{id}` | ✅ |
| Instructor.GetCourseCurriculum (+ _CourseCurriculumPartial) | `GET /courses/{id}/curriculum` ← `/instructor/courses/:id/curriculum` | ⚠️ كانت مش ممكن توصلها، ودلوقتي شغّالة |
| Instructor.UpsertCourseVideo / UploadVedio / DeleteVideo | `POST /sections/{id}/videos`، `DELETE /videos/{id}` | ⚠️ امتدادات الفيديو بس، والملف بيتحذف، وتعديل الفيديو اتشال |
| Instructor.UpsertReference / DeleteReference | `/sections/{id}/references`، `/references/{id}` | ⚠️ الحفظ بقى شغّال |
| Instructor.UpsertQuiz / DeleteQuiz / GetQuestions | `/sections/{id}/quizzes`، `/quizzes/{id}` ← صفحة الأسئلة | ✅ |
| Exam.Index / Upsert / Questions / UpSertQuestions | `/api/instructor/exams...` ← `/instructor/exams/...` | ⚠️ الحذف اتصلّح، والكورسات بتظهر في الإنشاء |
| Exam.UpSertListQuestions | — | ➖ الـ POST بتاعه كان متعلّق عليه في الأصل |
| Assinment.Index / UpSert / DeleteConfirmed / GetAssignmentDetails / GetSubmitions | `/api/instructor/...assignments...` ← `/instructor/assignments` | ⚠️ الملف اختياري في التعديل، وبيتحذف من الديسك |
| Assinment.DownloadAssignment | رابط الملف المباشر | ⚠️ الـ Endpoint اتشال بسبب Path traversal |
| Student.Index / InsertFeedBack | `/api/instructor/courses/{id}/students`، `/feedback` ← `/instructor/students` | ⚠️ طلاب كورساته بس |
| Questions.Index / GetQuestions | — | ➖ Controller مكرّر من غير `[Area]` ومكانش بيتوصله |
| Home.Index / Privacy (Instructor) | — | ➖ صفحات Template فاضية |

## Student
| Controller.Action | النسخة الجديدة | الحالة |
|---|---|---|
| DashBoard.Index | `GET /api/student/dashboard` ← `/student` | ✅ |
| Courses.Index | `GET /api/student/courses` ← `/student/courses` | ⚠️ Progress الطالب نفسه بس |
| DashBoard.CourseProcess / GetContent / GetVideo | `/api/student/courses/{id}`، `/sections/{id}` ← `/student/courses/:id` | ⚠️ لازم الطالب يكون مسجّل |
| DashBoard.GetQuestions / GetOne (+ GetQuestion) | `GET /sections/{id}/quiz` ← Quiz بعد 25% من الفيديو | ⚠️ كانت كود مش بيتنادى، دلوقتي شغّالة ومن غير الإجابات الصحيحة |
| DashBoard.QuickAccess | `GET /courses/{id}/quick-access` ← Modal | ✅ |
| DashBoard.UpsertFeedback / Delete | `/api/student/feedback` ← Modal بالنجوم | ⚠️ لصاحب الفيدباك بس، ومن غير Open redirect |
| StudentAssinment.UploadSoluation (+ _UploadSoluation) | `POST /assignments/{id}/submissions` ← خانة الحل | ⚠️ مكانش شغّال، ودلوقتي شغّال |
| StudentAssinment.Index | — | ➖ مكانش ليه View |
| DashBoard.CourseDetails | ← `/courses/:id` العامة | ➖ مفيش رابط كان بيوصله |
| Home.Index / Privacy (Student) | — | ➖ صفحات Template فاضية |

## Identity و Shared
| القديم | النسخة الجديدة | الحالة |
|---|---|---|
| Account/Login | `POST /api/auth/login` ← `/auth/login` | ✅ |
| Account/Register | `POST /api/auth/register` ← `/auth/register` | ⚠️ Student بس، ومن غير الدور الزيادة |
| Account/Logout | `POST /api/auth/logout` ← زرار الخروج | ✅ |
| Manage/Index (التليفون، الوصف، الصورة) | `/api/account/profile` ← `/account` | ✅ |
| Manage/ChangePassword | `PUT /api/account/password` ← `/account/password` | ✅ |
| باقي صفحات Identity (Forgot password، Email، 2FA، External logins، Personal data) | — | ➖ الـ EmailSender كان no-op، ودي في الـ Backlog |
| `_Layout`، `_InstructorLayout`، `_StudentLayout`، `_RegistreationLayout`، `_HomeLayout`، `_LoginPartial` | `layouts/*` و `app-user-menu` | ✅ نفس الـ CSS متعزل لكل Layout |
| CartIcon View Component | `CartStore` + Badge | ✅ |
| Categories View Component | `GET /api/catalog/nav` + Dropdown | ✅ |
| `_partialPagination` | `app-pagination` | ✅ |
| Views/Shared/Details (تفاصيل التراك) | `/admin/tracks/:id` و `/tracks/:id` | ✅ |

---

## الـ Cutover (لسه مستني موافقتك)
اتفقنا إن **محدش يحذف** أي حاجة من الحاجات دي غير لما تقول:
- `Banha UniverCity/` (الـ MVC)
- `DataAccess/` و `BFCAI.Models/` و `BFCAI.Utility/` (طبقات الـ MVC)
- `Models/` و `BFCAI.DataAccess/` (مشاريع ميتة مش في الـ .sln)

**قبل الحذف:**
1. انقل الملفات المرفوعة من `Banha UniverCity/wwwroot` (الفولدرات `Covers` و `videos` و `assignments` و `uploads` و `Images/profile_pics`) لـ `backend/src/BFCAI.Api/wwwroot`، أو غيّر `Storage:RootPath`.
2. على الداتابيز الحقيقية شغّل `backend/docs/database/attach-existing-database.sql` وبعده `dotnet ef database update`.
3. اعمل Rotate للمفاتيح اللي في Git history: Stripe و Google و Facebook و Gmail.
