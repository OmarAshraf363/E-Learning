# نبذة عن المشروع: منصة تعليم إلكتروني لكلية حاسبات بنها (BFCAI)

نظام إدارة تعلّم (LMS) لكلية الحاسبات والذكاء الاصطناعي بجامعة بنها، وفيه كمان جزء لبيع الكورسات أونلاين.

## التقنيات المستخدمة

- **ASP.NET Core 8 MVC** مع Razor Pages
- **Entity Framework Core 8** على **SQL Server** (وفيه باكدج SQLite متضافة)
- **ASP.NET Identity** لتسجيل الدخول والأدوار (روافد Google وFacebook موجودة بس متعلّق عليها في الكود)
- **Redis** للكاش (`StackExchange.Redis`)
- **Stripe** للدفع (`Stripe.net`)

## هيكل الـ Solution

| المشروع | دوره |
|---|---|
| `Banha UniverCity/` | تطبيق الويب: الـ Controllers والـ Views والملفات الثابتة، وفيه ملف الـ `.sln` |
| `DataAccess/` | الـ `DbContext` والـ Repositories والـ Unit of Work وطبقة Services |
| `BFCAI.Models/` | الـ Entities والـ ViewModels والـ Enums |
| `BFCAI.Utility/` | ثوابت الأدوار وحالات الدفع وحجم الصفحة وشوية Helpers (`StaticData.cs`) |
| `Models/` و`BFCAI.DataAccess/` | نسخ قديمة، ومشروع الويب مش بيعمل لهم Reference |

### طبقة الـ Services في `DataAccess`

`CourseService` و`VedioService` و`CartService` و`PaymentService` و`StripeGetWay` و`PostService` و`AssinmentService` و`ImageService` و`CacheService`

## الأدوار

`Admin` و`Instructor` و`Student` و`Customer` (متعرّفة في `BFCAI.Utility/StaticData.cs`)

## الأقسام (Areas) والمميزات

### Admin
إدارة الأقسام (Departments) والتراكات والكورسات والسنين الدراسية والمستخدمين والتسجيل في الكورسات (Enrollment) والجداول (Schedule)، وفيه لوحة تحكم.

### Instructor
إدارة كورساته، والواجبات، وبنك الأسئلة، والامتحانات، ومتابعة الطلاب.

### Student
لوحة تحكم، والكورسات اللي سجّل فيها مع متابعة التقدّم، وتسليم الواجبات.

### Customer (الواجهة العامة، وهي الـ Area الافتراضية)
- الصفحة الرئيسية وتصفّح الكورسات
- سلة مشتريات والدفع بـ Stripe
- **Community**: بوستات وكومنتات وتفاعلات (Reactions)

## الكيانات الأساسية (Entities)

- **أكاديمي:** `Department` و`Track` و`AcademicYear` و`Course` و`CourseCurriculum` و`CourseVideo` و`CourseResource` و`LearningObjective` و`TopicCovered` و`KeyWord`
- **التقييم:** `Exam` و`Question` و`QuestionChoice` و`ExamSubmission` و`Assignment` و`AssignmentSubmission`
- **التشغيل:** `Enrollment` و`StudentCourseProgress` و`Attendance` و`ClassSchedule` و`Room` و`Event` و`Feedback`
- **التجارة:** `Cart` و`CartItems` و`Order` و`OrderItems`
- **المجتمع:** `Community` و`Post` و`Comment` و`Reaction`
- **المستخدم:** `ApplicationUser` (بيورث من `IdentityUser`)

## آخر التطورات (من سجل الكوميتات)

- إضافة ميزة الـ Community وتحسين الـ Layout
- تحويل الاستعلامات من `Include` لـ Projection
- تحسين التصميم وتجاوبه مع الشاشات (Responsive)
- إضافة كاش بـ Redis
- **الخطوة الجاية المكتوبة:** ميزة Repost وShare

## التشغيل محليًا (متطلبات)

- .NET 8 SDK
- SQL Server: الـ Connection String في `Banha UniverCity/appsettings.json` متظبط على جهاز التطوير، فلازم تغيّره لجهازك
- Redis شغّال على `localhost:6379`
- مفاتيح Stripe (ويُفضّل تحطها في User Secrets)

```bash
cd "Banha UniverCity"
dotnet ef database update --project ../DataAccess
dotnet run
```

> أوامر التشغيل دي مبنية على قراءة الكود، ومش متجرّبة. ولو الـ Migrations مش جوه مشروع `DataAccess`، غيّر قيمة `--project`.

## ملاحظات ومشاكل محتاجة تتعالج

1. **فيه أسرار مرفوعة على الريبو:** `appsettings.json` فيه Stripe Secret Key وGoogle Client Secret، وفي `Program.cs` فيه Facebook AppSecret مكتوب صريح حتى لو السطر متعلّق عليه. لازم تلغي المفاتيح دي وتعمل غيرها، وتنقلها لـ User Secrets أو Environment Variables.
2. **مفيش `[Authorize]` على أي Controller** في أقسام Admin وInstructor وStudent، فأي حد يعرف الرابط يقدر يفتح صفحات الأدمن.
3. **فيه مشاريع قديمة مش مستخدمة** (`Models/` و`BFCAI.DataAccess/`) ومحتاجة تتشال.
4. **فيه ملفات فيديو وصور متخزنة جوه الريبو** في `wwwroot/videos` و`wwwroot/Covers`، ودي بتكبّر حجم الريبو. الأحسن يتحطوا في `.gitignore` أو يتنقلوا لـ Blob Storage.
5. الثابت `Comming_Status` قيمته `"In-Process"` زي `In_Process_Status` بالظبط، والأغلب إنها غلطة.
6. مفيش Tests ولا README للمشروع.
