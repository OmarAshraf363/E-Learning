# حالة التنفيذ

| Phase | الحالة |
|---|---|
| 0: الأساسيات (Solution و DB baseline و Auth infra و Angular skeleton و Docs) | ✅ خلصت |
| 1: Auth و Users | ✅ خلصت |
| 2: Admin | ✅ خلصت |
| 3: Public catalog | ✅ خلصت |
| 4: Cart و Payment | ⏳ |
| 5: Instructor | ⏳ |
| 6: Student | ⏳ |
| 7: Community | ⏳ |
| 8: Parity و Cutover | ⏳ |

### نتيجة التحقق من الداتابيز (Phase 0)
- اتعمل Schema من الـ Migrations القديمة (42 Migration) وSchema تانية من الـ Baseline الجديدة، واتقارنوا: الأعمدة، والأنواع، والـ Nullability، والـ Defaults، والـ FKs ومعاها Delete rules، والـ Indexes. **متطابقين**، والفرق الوحيد جدول `RefreshTokens`.
- اتجرّب سكربت `backend/docs/database/attach-existing-database.sql` على نسخة من الداتابيز القديمة، وبعده `database update`. النتيجة طلعت **متطابقة 100%** مع الداتابيز الجديدة.

### Phase 1: اللي اتنقل
| MVC | API | Angular |
|---|---|---|
| Identity Login | `POST /api/auth/login` | `/auth/login` |
| Identity Register (زائر → Student) | `POST /api/auth/register` | `/auth/register` |
| Home/PostRegister ("Teach in BFCAI") | `POST /api/auth/register-instructor` | `InstructorRegisterModalComponent` (هيتحط في الـ Home في Phase 3) |
| — | `POST /api/auth/refresh` و `POST /api/auth/logout` و `GET /api/auth/me` | الـ Auth interceptor (Refresh تلقائي) |
| Manage/Index (التليفون، الوصف، الصورة) | `GET/PUT /api/account/profile` | `/account` |
| Manage/ChangePassword | `PUT /api/account/password` | `/account/password` |
| Admin/User (Index و Upsert و Lock و Unlock و Delete) + Register الأدمن | `/api/admin/users` (+ `/roles` و `/{id}/lock` و `/{id}/unlock`) | `/admin/users?filter=` و `/admin/users/new` و `/admin/users/:id` |

**الإصلاحات التقنية في Phase 1:**
- الـ Register العام بيعمل Student بس.
- اليوزر مبقاش بياخد دور Student زيادة على دوره.
- `UserType` بيتظبط في كل طرق إنشاء اليوزرز وتعديل الدور.
- إدارة اليوزرز بقت للأدمن بس.
- الـ Lock والـ Delete بقوا `POST` و `DELETE` بدل GET.
- صورة البروفايل بقت لازم تكون صورة.
- لو حذف يوزر اتمنع بسبب داتا مرتبطة بيه، الـ API بيرجّع 409 برسالة واضحة بدل 500.

**التحقق:**
- 18 Integration test: Login، و Register، و Register instructor، و Refresh rotation، و Logout، و Lock، وتغيير الدور، والحذف، والبروفايل، وتغيير الباسورد، و 403 للطالب.
- E2E في Chromium على الـ API الحقيقية و SQL Server: Login الأدمن، وإنشاء يوزر، و Lock، ومنع اليوزر المقفول، وأخطاء الـ Validation (من الـ Client ومن الـ Server)، و Register، ومنع الطالب من `/admin`، وتحديث البروفايل، و Refresh التوكن تلقائي.

### Phase 2: اللي اتنقل
| MVC (Admin) | API | Angular |
|---|---|---|
| DashBoard/Index | `GET /api/admin/dashboard?page=` | `/admin` |
| Department (Index و UpSert و Details و Courses و Delete) | `/api/admin/departments` (+ `/{id}/details` و `/{id}/courses`) | `/admin/departments` و `/admin/departments/:id` و `/admin/departments/:id/courses` (والإضافة والتعديل في Modal) |
| Track (Index و UpSert و Details و Delete) | `/api/admin/tracks` (+ `/{id}/details`) | `/admin/tracks` و `/admin/tracks/:id` |
| AcadmicYear | `/api/admin/academic-years` | `/admin/academic-years` |
| Course (Index و Details و UpSert و Delete) | `/api/admin/courses` (+ `/{id}/details`) | `/admin/courses` و `/admin/courses/new?deptId=&trackId=` و `/admin/courses/:id` و `/admin/courses/:id/details` |
| Schedule (Index و UpSert) | `/api/admin/schedules` | `/admin/schedules` و `/admin/schedules/new` و `/admin/schedules/:id` |
| Enrollment (Index و EnrollByEmail و UploadCsv) | `GET /api/admin/enrollments` و `POST /by-email` و `POST /csv` | `/admin/enrollments` |
| (Dropdowns) | `/api/admin/lookups/{departments,tracks,courses,rooms,academic-years,instructors,users}` | `LookupsApi` |

**الإصلاحات التقنية في Phase 2:**
- **إضافة Key Word و Objective و Topic في فورم الكورس:** الحقول الجديدة كانت بتتعمل من غير `name`، فعمرها ما كانت بتتحفظ. دلوقتي القوايم بتتضاف وتتشال وبتتحفظ فعلًا.
- **تعديل الكورس أو القسم أو التراك من غير صورة جديدة** كان بيمسح الغلاف القديم. دلوقتي الغلاف القديم بيفضل.
- **الغلاف** بقى إجباري عند الإنشاء بس، ولازم يكون صورة.
- **الـ Course upsert** كان بيعمل حوالي 6 `Commit` منفصلين، بقى `SaveChanges` واحد، يعني Transaction واحدة.
- **السنة الدراسية** كانت مش بتتحفظ من الـ Modal بسبب الـ return قبل الـ `Commit`.
- **الجداول:** الـ Instructor بقى بيتحفظ بالـ Id الحقيقي. الـ `int.Parse` على GUID كان بيمنع الحفظ.
- **الداشبورد:**
  - "Total Courses" كان بيعرض حجم أول صفحة بس.
  - "Most Popular Course" كان ديمًا فاضي.
  - الاتنين بيعرضوا القيم الحقيقية دلوقتي.
- **CSV:**
  - السطر الغلط بقى بيتخطّى بدل ما يوقع الـ Upload كله.
  - أول عمود بيقبل Id أو Email، زي ما كان مكتوب في شاشة الرفع.
- **الكاش:** أي تعديل في الكورسات أو الأقسام أو التراكات بيمسح كاش الصفحة الرئيسية.
- **الحذف:** لو فيه داتا مرتبطة، الـ API بيرجّع 409 برسالة واضحة.

**تغييرات بسيطة في الواجهة:**
- اتضاف زرار "New Track"، لأن الصفحة القديمة مكانش فيها طريقة لإضافة تراك غير بكتابة الرابط بإيدك.
- خانة البحث في جداول الكورسات والأقسام بقت شغّالة فعلًا (فلترة محلية). قبل كده كانت شكل بس.
- زرار "Enroll Manually" اتشال، لأنه كان Modal فاضي مالوش Action.

**التحقق:**
- 33 Integration test.
- E2E في Chromium على الـ API الحقيقية و SQL Server، وكل الخطوات نجحت:
  - الداشبورد.
  - قسم: إجبار الغلاف، الإضافة، التعديل من غير غلاف، صفحة التفاصيل.
  - إضافة تراك.
  - كورس: الإضافة بقوايم الـ Keywords والـ Objectives والـ Topics، والتعديل وشيل Keyword مع بقاء الغلاف.
  - تفاصيل التراك.
  - سنة دراسية.
  - جدول بـ Instructor.
  - التسجيل بالإيميل (خطأ ونجاح).
  - استيراد CSV.

### Phase 3: اللي اتنقل
| MVC (Customer/Home) | API (`/api/catalog`, anonymous) | Angular |
|---|---|---|
| Index (Banner و Top courses و Category tabs و Top categories و Tracks و Recommendations و Testimonials و Events) | `GET /home?page=` (والكاش في Redis) | `/` |
| GetByDepTId (Category tabs) | `GET /departments/courses?departmentId=&page=` | جوه الـ Home |
| Courses + _partialFilterView | `GET /courses?search=&categoryId=&rate=&price=&instructorId=&sortBy=&sortOrder=&page=` | `/courses?...` |
| Details + GetObjectives + GetTopics | `GET /courses/{id}?starFilter=` | `/courses/:id` |
| Category (من غير id ومعاه id) | `GET /categories` و `GET /categories/{id}` | `/category` و `/category/:id` |
| TopicCourse | `GET /topics/{word}` | `/topic/:word` |
| TrackDetails | `GET /tracks/{id}` | `/tracks/:id` |
| Categories View Component (قايمة الـ Navbar) | `GET /nav` | Dropdown في الـ Public layout |
| GetRegister / PostRegister (Teach on BFCAI) | `POST /api/auth/register-instructor` (Phase 1) | Modal في الـ Home |
| تحويل الأدمن والـ Instructor من الـ Home | — | `homeRedirectGuard` |

**الإصلاحات التقنية في Phase 3:**
- **فلاتر صفحة الكورسات:**
  - الصفحة القديمة كانت بتحسب الفلتر وتعرض الليستة من غيره.
  - فلتر "Rating & Up" مكانش متطبّق.
  - دلوقتي الاتنين شغالين: البحث، والقسم، والسعر، والتقييم، والـ Instructor، والترتيب.
- **الكاش:**
  - التراكات كانت بتتقري من مفتاح وبتتكتب في مفتاح تاني، فالكاش عمره ما كان بيشتغل.
  - الكاش بقى بيخزّن DTOs بدل Entities فيها علاقات دايرية.
- **Progress التراك للطالب** كان ديمًا 0% لأن العدد عمره ما اتحسب.
- **التوصيات:** الكورس اللي مالوش Track أو Department مبقاش بيطابق كل الكورسات التانية اللي مالهاش (مشكلة `Contains(null)`).
- **Course details:** لو الكورس مش موجود بيرجّع 404 بدل Exception.
- **الفيديو التعريفي:** أول فيديو في المنهج زي الأصل، ولو مفيش بيستخدم `DemoVideoUrl` اللي الأدمن بيدخّله.
- **روابط الكورسات** في الـ Testimonials والـ Recommendations والـ Related كانت بتودّي على صفحة الكورسات من غير ما تفتح الكورس، دلوقتي بتفتح تفاصيله.
- **رابط `localhost:5197`** اللي كان مكتوب صريح اتشال.
- **الكورس اللي مالوش صورة** بيعرض صورة افتراضية.

**اتشال من الواجهة (كان شكل بس):**
- قايمة ترتيب الـ Reviews، وأزرار Helpful و Load more: كانت بتنادي Functions مش موجودة.
- زرار "More Info" في جدول الأحداث: كان رابطه غلط.

**التحقق:**
- 39 Integration test، منهم الكاش: إنه بيتملى، وبيتمسح لما الكتالوج يتغيّر.
- E2E في Chromium، وكل الخطوات نجحت:
  - الـ Home.
  - الـ Hover popover.
  - تبديل تابات الأقسام.
  - قايمة الـ Navbar.
  - البحث من الـ Header.
  - فلتر القسم.
  - صفحة الكورس، وفلتر النجوم، وتاب الـ Topics.
  - صفحة القسم.
  - صفحة الـ Topic.
  - تسجيل Instructor من "Teach on BFCAI".
  - تحويل الـ Instructor من `/` لـ `/instructor`.

### ملاحظات
- **صور البروفايل القديمة:** الـ MVC كان بيحفظها في فولدر `Images/profile_pics` لكن بيخزّن الرابط `/images/...` بحرف صغير. ده كان شغّال على Windows بس. النسخة الجديدة بتخزّن `/Images/profile_pics/...`. لو هتشغّل على Linux بداتا قديمة، غيّر اسم الفولدر أو حدّث القيم دي.
- **Stripe.net 53:** `PaymentMethodTypes` اتشال من الـ API بتاعتهم. طرق الدفع دلوقتي بتتحدد من Stripe Dashboard، وكارت الائتمان متاح بشكل افتراضي.
- روابط Grades و Reports و Settings الميتة في Sidebar الـ Instructor مش موجودة في النسخة الجديدة، ومتسجّلة في الـ Backlog.

---

# خطة نقل BFCAI من MVC إلى ASP.NET Core API (Clean Architecture) و Angular

## الخلفية (Context)
المشروع الحالي ASP.NET Core 8 MVC، وكل حاجة فيه متلزّقة في بعض: اللوجيك موجود في الـ Controllers والـ Views (98 View) والـ jQuery، ومفيش Authorization خالص. الهدف نطلع نسخة جديدة فيها:
- **API** على **.NET 10 LTS** مبنية بـ Clean Architecture.
- **Angular** بأحدث إصدار مستقر، متقسّمة Feature-based، وفيها Generic services.

النسختين لازم يوصلوا **لنفس النقطة اللي الـ MVC واصلها دلوقتي، وبنفس اللوجيك**.

القيود:
- مفيش over-engineering، والكود يكون سهل يتفهم ويتطوّر.
- مفيش أي قرار بيزنس في المرحلة دي. أي حاجة فيها قرار بيزنس تتكتب في Backlog للخطة التانية.

### القرارات اللي اخترتها
| الموضوع | القرار |
|---|---|
| .NET | .NET 10 LTS |
| مكان الكود | نفس الريبو: `backend/` و`frontend/`، والـ MVC يفضل زي ما هو كمرجع لحد الـ Cutover |
| شكل الـ Clean Architecture | 4 طبقات (Domain / Application / Infrastructure / Api) مع Services عادية، من غير MediatR |
| الـ Authentication | ASP.NET Identity مع JWT Access Token و Refresh Token |
| الداتابيز | نفس الـ Schema بالظبط، ونبدأ بـ Baseline Migration (الإضافة الوحيدة جدول `RefreshTokens`) |
| الوصول للداتا | Generic Repository و UnitOfWork |
| الـ Mapping | يدوي / Projection من غير مكتبة |
| الـ Validation | FluentValidation |
| الأخطاء | Exceptions مخصّصة وGlobal Handler بيرجّع ProblemDetails |
| الـ Endpoints | Controllers |
| الـ UI | Bootstrap 5.3 و ng-bootstrap و SweetAlert2، ونفس الـ CSS والألوان الحالية |
| الـ State | Signals جوه Store services لكل Feature |
| ترتيب الشغل | Phases حسب الـ Feature، وكل Phase فيها API و Angular مع بعض |
| بيئة التشغيل | docker-compose لـ SQL Server و Redis |
| الـ Tests | xUnit للـ Services الحساسة، و Integration tests قليلة |
| الـ Bugs الحالية | نصلّح التقني والأمني بس، والبيزنس يروح للـ Backlog |

---

## 1. هيكل الـ Backend (`backend/`)
```
backend/
  BFCAI.slnx
  docker-compose.yml               # SQL Server + Redis
  src/
    BFCAI.Domain/                  # Entities, Enums, Constants (Roles, PaymentStatus...)
    BFCAI.Application/
      Common/  Interfaces/ (IGenericRepository<T>, IUnitOfWork, ICurrentUser, IFileStorage,
                            ICacheService, IPaymentGateway, ITokenService, IIdentityService)
               Exceptions/ (NotFound, Forbidden, BadRequest, Validation)
               Models/ (PagedResult<T>, PaginationParams, CourseParams)
      Features/<Feature>/  Dtos/  I<Feature>Service.cs  <Feature>Service.cs  Validators/
      DependencyInjection.cs
    BFCAI.Infrastructure/
      Persistence/ AppDbContext.cs, Configurations/ (IEntityTypeConfiguration per entity),
                   Migrations/, Repositories/ (GenericRepository, UnitOfWork, + specific repos)
      Identity/ (IdentityService, JwtTokenService, RefreshToken)
      Files/ LocalFileStorage.cs     Caching/ RedisCacheService.cs
      Payments/ StripePaymentGateway.cs
      DependencyInjection.cs
    BFCAI.Api/
      Controllers/ Auth/, Admin/, Instructor/, Student/, Public/, Community/
      Infrastructure/ GlobalExceptionHandler.cs, CurrentUser.cs
      Program.cs  (JWT, CORS, OpenAPI + Scalar, static files for uploads)
  tests/
    BFCAI.Application.Tests/       # xUnit + SQLite in-memory (real repos, no heavy mocking)
    BFCAI.Api.IntegrationTests/    # WebApplicationFactory
  docs/ ARCHITECTURE.md, ADD-A-FEATURE.md
```

**اتجاه الاعتماد بين الطبقات:** Api ← Infrastructure ← Application ← Domain. الـ Application ميعرفش حاجة عن EF ولا Stripe ولا Redis.

**`Domain`:** فيه `ApplicationUser : IdentityUser` (بنقبل اعتماد على `Microsoft.Extensions.Identity.Stores` عشان نحافظ على نفس الـ Schema).

**`Controller`:** بيعمل حاجة واحدة: يستدعي الـ Service ويرجّع النتيجة. مثلًا:
```csharp
[HttpGet("{id}")] public Task<CourseDto> Get(int id) => _service.GetAsync(id);
```

**`Service`:** كل البيزنس لوجيك المنقول بيعيش هنا. بتستخدم `IUnitOfWork` و `ICurrentUser`، وبترمي Exceptions لما يحصل خطأ.

**Generic Repository:** نبدأ من الـ API الحالي في `DataAccess/Repository/IRepository/IGenralRepository.cs` ونخليه async بالكامل:
- `GetAllAsync`, `GetOneAsync`, `Query()`, `AddAsync`, `Update`, `Delete`, `AddRange`, `DeleteRange`
- `UnitOfWork.SaveChangesAsync()`

**Repositories خاصة:** بنعملها بس للـ Entities اللي ليها استعلامات معقّدة النهارده. الـ Projections بتتنقل زي ما هي من:
- `CourseRepository`: `GetCourseCurriculum`, `GetProcess`, `QuickAccessToSpacifcCourse`, `GetFilteredCourses`, `GetListOfCoursesDetailsWithPagination`
- `CourseCurriculumRepository`
- `DepartmentRepository`: `getSpacifcDetails`, `GetTopDepartments`
- `TrackRepository`: `TrackDetailsProjection`, `GetTopTracksAsync`
- `EnrollmentRepository`
- `KeyWord` / `LearningObjective` / `TopicCovered`: الـ `Add*InTable` helpers

**Services بتتنقل للـ Application بنفس اللوجيك:** `CourseService`, `CartService`, `PaymentService`, `PostService`.
- اللوجيك اللي حاليًا جوه الـ Controllers بيتسحب لـ Services جديدة: Department, Track, AcademicYear, Schedule, Enrollment, Users, Community, Exam, Curriculum, Assignment, Feedback.
- رفع الملفات (اللي في `VedioService` و `AssinmentService` و `ImageService` ومعاها الكود اللي في الـ Controllers) بيتجمّع في `IFileStorage` واحد، وفيه Whitelist لامتدادات كل نوع ملف وحدود الحجم الحالية.

### الداتابيز
- الـ Entities والـ Fluent configuration بتتنقل من `DataAccess/Data/ApplicationDbContext.cs` بنفس:
  - أسماء الجداول (منها الأسماء المفردة زي `Track`, `Feedback`, `AcademicYear`)
  - الـ FKs والـ DeleteBehavior
  - الـ Discriminator اللي على `AspNetUsers` (`IdentityDbContext<IdentityUser>` + `ApplicationUser`)
- **Migration رقم 1 `Baseline`:**
  - على أي داتابيز موجودة، بنضيف صفها في `__EFMigrationsHistory` من سكربت موجود في `docs/`، فمش هتتنفّذ عليها.
  - بنصلّح الـ Seed اللي بيتغيّر كل مرة (`DateTime.Now` وهاش باسورد الأدمن) ونخليه قيم ثابتة **بنفس البيانات**.
- **Migration رقم 2:** إضافة جدول `RefreshTokens`.
- **التحقق:**
  1. نعمل `dotnet ef migrations script` من المشروع القديم والجديد.
  2. نقارن الـ DDL بين الاتنين.
  3. لازم يكون مفيش أي فرق غير جدول `RefreshTokens`.

### الـ Auth
**الـ Endpoints:**
- `POST /api/auth/login`
- `POST /api/auth/register` (طالب)
- `POST /api/auth/register-instructor` (نفس `HomeController.PostRegister` الحالي)
- `POST /api/auth/refresh`
- `POST /api/auth/logout`
- `GET /api/auth/me`
- `PUT /api/account/profile`
- `PUT /api/account/password`

**التوكن:** الـ JWT فيه `sub` و `role` و `name`. الـ Policies بتتعمل بالأدوار الحالية (`Admin` / `Instructor` / `Student`).

**الـ Lockout:** الـ Lock/Unlock الحالي بيكمّل يشتغل بـ `CheckPasswordSignInAsync` مع lockout.

---

## 2. هيكل الـ Frontend (`frontend/`)
**إعداد المشروع:**
- Angular (أحدث إصدار مستقر وقت الإنشاء) بـ `ng new`.
- Standalone و Zoneless و Signals.
- الـ Control flow الجديد (`@if` / `@for`).
- Typed Reactive Forms.
- Interceptors و Guards مكتوبين كـ functions.
```
src/app/
  core/
    api/        api-client.ts (HttpClient wrapper: url + HttpParams + FormData)
                base-api.service.ts  -> BaseApiService<T, TCreate, TUpdate>
                                        getAll(params) / getById / create / update / delete
    state/      crud-store.ts        -> CrudStore<T>: items/selected/loading/error signals + load/save/remove
    auth/       auth.service.ts, token.storage.ts, auth.interceptor.ts (attach + refresh on 401),
                auth.guard.ts, role.guard.ts
    errors/     error.interceptor.ts (ProblemDetails -> SweetAlert/field errors)
    models/     paged-result.ts, problem-details.ts
  shared/       components/ (pagination, course-card, star-rating, file-upload, form-errors,
                confirm dialog service on SweetAlert2), pipes/ (file-url)
  layouts/      public-layout, admin-layout, instructor-layout, student-layout, community-layout
  features/
    auth/ public/ (home, courses, course-details, category, topic, track) cart/ community/
    admin/ instructor/ student/
      <feature>/
        data-access/  <feature>.api.ts (extends BaseApiService)  <feature>.store.ts (signals)
        pages/        (routed, smart)
        components/   (presentational, input()/output())
        models/
        <feature>.routes.ts   (lazy loaded)
  styles/ _variables.scss (نفس ألوان --primary-color #6366f1 ...), ومحتوى ملفات CSS الحالية
          (Home.css, studentLay.css, InstructorLayout.css, Comunity.css, site.css)
docs/ ADD-A-FEATURE.md
```
**الـ Layouts:**
- نفس الـ Layouts والـ Menus الحالية.
- الـ Redirect حسب الدور (اللي حاليًا جوه `HomeController.Index`) بيبقى `roleGuard` / redirect بعد الـ Login.

**ملفات الميديا:**
- الـ URLs بتتبني من `environment.fileBaseUrl` بدل `localhost:5197` المكتوب صريح.
- الـ Modals اللي كانت بتجيب HTML partials عن طريق AJAX بتبقى Components جوه `NgbModal`، وبتجيب الداتا كـ JSON.

---

## 3. الـ Phases
كل Phase:
- بتتعمل API و Angular و Tests مع بعض.
- ليها Commit منفصل.
- بتتراجع منك قبل ما نبدأ اللي بعدها.

**Phase 0: الأساسيات**
- الفولدرات والـ Solution و docker-compose و `.editorconfig`.
- الـ Domain والـ DbContext والـ Baseline Migration، ومعاها التحقق من إن الـ Schema متطابقة.
- الـ API skeleton: ProblemDetails و Scalar و CORS و JWT و CurrentUser و FileStorage و Cache.
- Angular skeleton: core و layouts و auth و Bootstrap.
- `docs/` (ARCHITECTURE و ADD-A-FEATURE).
- نقل الأسرار لـ `user-secrets` أو env.

**Phase 1: Auth و Users**
- Login و Register و Instructor register و Refresh و Profile و Change password.
- Admin Users: List/filter حسب `UserType`، Upsert بالـ Role، Lock/Unlock، Delete.

**Phase 2: Admin**
- Dashboard.
- Departments (CRUD و Details و Courses).
- Tracks و AcademicYears.
- Courses: Upsert مع Cover و Keywords و Objectives و Topics.
- Schedules.
- Enrollments: بالإيميل وبـ CSV.

**Phase 3: الواجهة العامة (Public catalog)**
- Home مع كاش Redis: top-courses و departments و tracks.
- Courses: filter و sort و paging.
- Course details: reviews مع فلتر النجوم، related courses، objectives و topics.
- Category و Topic و Track details.
- قايمة الأقسام في الـ navbar.

**Phase 4: Cart و الدفع**
- Cart و Add و Remove و عدّاد السلة.
- Stripe Checkout و صفحتي Success و Cancel.

**Phase 5: Instructor**
- Dashboard و My courses (paged) و Course upsert.
- Curriculum: sections و رفع videos و references و quizzes.
- Exams و Questions.
- Assignments: upload و details و download و delete و submissions.
- Students و Feedback.

**Phase 6: Student**
- Dashboard: الكورسات، الجدول، التوصيات، الـ related posts، الـ feedback.
- My courses.
- Course player: المحتوى، الفيديو، الأسئلة بعد 25% من الفيديو.
- Quick access.
- Feedback upsert/delete مع إعادة حساب الـ Rate.
- Assignment submission.

**Phase 7: Community**
- Feed بالفلاتر: community و word و maxReact و user.
- Posts CRUD مع attachment و keywords.
- Comments و replies.
- Reactions: toggle و counts و lists، و Top instructors.

**Phase 8: Parity و Cutover**
- Checklist بنقارن فيها صفحة بصفحة مع الـ MVC.
- بعد موافقتك: نشيل الـ MVC والفولدرات الميتة (`Models/` و `BFCAI.DataAccess/`) ونحدّث `PROJECT_BRIEF.md`.

---

## 4. إصلاحات تقنية وأمنية هتتعمل أثناء النقل (مش قرارات بيزنس)

**الـ Authorization والأمان:**
- `[Authorize(Roles=...)]` على كل Area.
- ملكية الموارد: الـ Instructor يعدّل كورساته وامتحاناته وواجباته بس، وكل يوزر يعدّل بوستاته وكومنتاته وسلّته وفيدباكه بس.
- الـ `UserId` و `InstructorId` و `ProviderUserId` بتتاخد من التوكن، مش من الـ Body.
- الـ Register العام بيعمل Student بس. تحديد الـ Role متاح للـ Admin بس.
- الـ Register كان بيضيف دور Student لأي حد زيادة على دوره، وكان بيضيف الدور قبل ما اليوزر يتعمل. الاتنين اتصلّحوا.
- `UserType` بيتظبط في كل طرق إنشاء اليوزرز، عشان الفلترة الحالية تشتغل.
- محتوى الكورس (CourseProcess و GetVideo) متاح للمسجّلين في الكورس، وللـ Instructor بتاعه، وللـ Admin.
- `DownloadAssignment` فيه Path traversal واتقفل.
- Whitelist لامتدادات الملفات في كل الـ uploads.
- Open redirect في `Delete` بتاع الـ Feedback اتشال.
- العمليات اللي كانت بتغيّر داتا عن طريق GET بقت `POST` / `PUT` / `DELETE`.

**Stripe:**
- الـ Success URL بقى فيه `{CHECKOUT_SESSION_ID}`، والـ Session بتتجاب من Stripe ونتأكد إن `payment_status == paid` **قبل** التسجيل في الكورس.
- `StripeChargeId` بيتحفظ فعلًا (كان بيضيع بسبب `AsNoTracking`).

**Bugs بتكسر اللوجيك المقصود:**
- Actions كانت بترجّع قبل `Commit` فمش بتحفظ: AcademicYear و CourseVideo و Reference upsert.
- السلة كانت بتجمع السعر مرتين (`Price +=` مع `Quantity +=`)، وزيادة الكمية مكانتش بتتحفظ.
- `Schedule.InstructorId` كان بيتعمله `int.Parse` على GUID.
- نتيجة فلتر `Courses` كانت بتتجاهل.
- مفتاح كاش التراكات مش متطابق (`top-tracks` / `top-tracks-v2`).
- الكاش كان بيخزّن Entities فيها Cycles، وبقى بيخزّن DTOs.
- الكاش بيتمسح لما Course أو Department أو Track يتعدّل.
- `CourseService.SaveCourseAsync` مكانش بيحط `DepartmentId`، وغلاف الكورس مكانش بيتحدّث لو مكانش ليه غلاف قبل كده.
- مسارات حذف الملفات (cover و assignment و video و attachment) كانت غلط.
- تعديل البوست كان بيمسح الـ Keywords ومش بيضيف الجداد.
- `ViewBag.prog` بقى بيجيب Progress اليوزر بس.
- Validation الصورة الإجبارية كانت متجاهلة.
- الـ Seed مش ثابت.
- `QuestionsController` مكرّر ومن غير Area، فمش هيتنقل.

**الأسرار:** بتتنقل من `appsettings.json` و `Program.cs` و `StaticData.SendConfirmationEmail`.
> ⚠️ لازم **إنت** تعمل Rotate لمفاتيح Stripe و Google و Facebook و Gmail، لأنها موجودة في تاريخ Git.

## 5. Backlog للخطة التانية (قرارات بيزنس، مش هتتعمل دلوقتي)
- **التسجيل والجداول:**
  - منع التسجيل المكرر (Cart و Enroll by email و CSV)
  - التحقق إن اليوزر Student
  - تعارض الجداول
  - Rooms CRUD
- **الامتحانات والواجبات:**
  - تسليم الامتحان وتصحيحه (مش موجود حاليًا)
  - شرط إجابة صحيحة واحدة على الأقل
  - Deadline الواجبات
  - رفع ملف حل الطالب
- **الدفع والإيميل:**
  - Webhooks و Refunds و حالة `Success-Confirmed`
  - إيميل التأكيد و Forgot password (الـ EmailSender حاليًا no-op)
  - Google و Facebook login و 2FA
  - الـ Order اللي بيتعمل Cancel
  - الـ ×1.1 في إجمالي السلة
- **ميزات ناقصة في الـ UI:**
  - `sortReviews` و `loadMoreReviews` و `markHelpful` و `startExam`
  - روابط Grades و Reports و Settings الميتة
- **تنظيف الـ Schema:**
  - الأسماء المفردة للجداول
  - `Reaction.Type` نص بدل Enum
  - تكرار `UserType` مع الأدوار
  - الـ Discriminator
- **حسابات اليوزرز (موجودة في صفحات Identity الافتراضية، ومش مستخدمة فعليًا لأن الـ EmailSender no-op):**
  - Remember me
  - Forgot password
  - تغيير الإيميل
  - External logins
  - Personal data (download/delete)
- **حماية الأدمن:** منع الأدمن إنه يحذف أو يقفل حسابه هو.
- **زرار "Add to Cart"** بيودّي على `/cart?add={id}`، زي الـ MVC اللي كان بيعمل `Cart/AddToCart` وبعدين يفتح السلة. الصفحة نفسها في Phase 4.
- **إدارة الأحداث (Events):** الداشبورد وتفاصيل القسم بيعرضوا الأحداث، بس مفيش شاشة تضيفها، لا قبل ولا دلوقتي.
- **قائمة الـ Instructor في فورم الجداول** بتعرض كل اليوزرز زي الـ MVC. محتاجة قرار: نخليها Instructors بس؟
- **حاجات تانية:**
  - Localization و RTL
  - تخزين الفيديوهات على Blob
  - Repost و Share في الـ Community

---

## 6. التحقق (Verification)
- **Schema:** مقارنة DDL الـ Migrations القديمة مع الجديدة. لازم يكون مفيش فرق غير `RefreshTokens`.
- **كل Phase:**
  - `dotnet build`
  - `dotnet test`: Unit tests للـ Services الحساسة (Cart و Payment مع Stripe fake و Auth و Enrollment و Feedback rating و Community reactions)، و Integration tests للـ Auth والـ Authorization (401/403/ownership).
  - `ng build`
  - `ng test`
- **Smoke من أول لآخر:**
  1. `docker compose up -d`
  2. `dotnet run --project backend/src/BFCAI.Api`، وبعدها نجرّب في Scalar.
  3. `ng serve`
  4. Playwright: سيناريو لكل دور (Admin / Instructor / Student / Visitor)، ولقطات للصفحات المقابلة في الـ MVC والـ Angular.
- **Phase 8:** Parity checklist لكل الـ 98 View والـ Actions اللي اتحصرت، وكل واحدة متعلّم عليها ✅.

## ملاحظات تنفيذية
- الشغل على الفرع `claude/peaceful-edison-18c8gj`. الـ Push حاليًا متوقف لحد ما صلاحية GitHub تتظبط (https://claude.ai/connect-github).
- لو SQL Server أو Docker مش متاحين في بيئة السحابة، الـ Tests بتشتغل على SQLite، والتجربة الكاملة بتتعمل عندك محليًا بـ docker-compose.
