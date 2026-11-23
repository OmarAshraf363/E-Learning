# نبذة عن المشروع: منصة تعليم إلكتروني لكلية حاسبات بنها (BFCAI)

نظام إدارة تعلّم (LMS) لكلية الحاسبات والذكاء الاصطناعي بجامعة بنها، وفيه كمان جزء لبيع الكورسات أونلاين.

> **حالة المشروع:** المشروع اتنقل من ASP.NET Core 8 MVC لـ **API بـ .NET 10 (Clean Architecture)** و **Angular 22**، ووصل لنفس النقطة اللي كان واصلها الـ MVC.
> كود الـ MVC القديم **هيفضل موجود في الريبو** (قرار نهائي، مش هيتحذف).
> الخطة التانية (قرارات البيزنس) شغّالة في `docs/SECOND_PLAN.md`، والمسار أ منها اتعمل.

## هيكل الريبو
| الفولدر | المحتوى |
|---|---|
| `backend/` | الـ API الجديدة: .NET 10 و Clean Architecture و EF Core و SQL Server و Redis و Stripe و JWT |
| `frontend/` | Angular 22: Standalone و Signals و Zoneless، ومتقسّمة Feature-based، و Bootstrap 5.3 و ng-bootstrap |
| `e2e/` | 7 سيناريوهات Playwright بتغطي كل المناطق |
| `docs/` | خطة النقل وحالتها (`MIGRATION_PLAN.md`) و Parity checklist والخطة التانية (`SECOND_PLAN.md`) |
| `Banha UniverCity/` و `DataAccess/` و `BFCAI.Models/` و `BFCAI.Utility/` | الـ MVC القديم (فاضل في الريبو) |
| `Models/` و `BFCAI.DataAccess/` | مشاريع قديمة ميتة، مش في الـ .sln |

## التقنيات (النسخة الجديدة)
- **Backend:**
  - .NET 10 LTS، و ASP.NET Core Controllers، و EF Core 10 على SQL Server.
  - نفس الـ Schema القديمة، والإضافات: جدول `RefreshTokens` وعمود `CourseCurricula.SortOrder`.
  - ASP.NET Identity مع JWT و Refresh tokens.
  - FluentValidation، والأخطاء بترجع ProblemDetails.
  - Redis للكاش، و Stripe Checkout للدفع، و Scalar لتوثيق الـ API.
- **Frontend:**
  - Angular 22.
  - Generic `BaseApiService` و `CrudStore`، وكل Feature فيها Store بالـ Signals.
  - Interceptors للـ Auth والـ Refresh والأخطاء.
  - SweetAlert2.
  - نفس ألوان وستايل المشروع القديم، متقسّمة على الـ Layouts.

## الأدوار والمناطق
| المنطقة | الدور | أهم الصفحات |
|---|---|---|
| عامة (`/`) | الكل | الرئيسية، والكورسات بالفلاتر، وتفاصيل الكورس، والأقسام، والتراكات، و Teach on BFCAI |
| السلة (`/cart`) | أي يوزر مسجّل دخول | السلة والدفع بـ Stripe (وبيتأكد من الدفع قبل التسجيل) |
| الأدمن (`/admin`) | Admin | الداشبورد، واليوزرز، والأقسام، والتراكات، والكورسات، والسنين، والجداول، والتسجيل |
| الـ Instructor (`/instructor`) | Instructor | كورساته، والمنهج (فيديو ومراجع وواجبات وكويزات)، والامتحانات والأسئلة، والواجبات، والطلاب |
| الطالب (`/student`) | Student | الداشبورد، وكورساتي، والـ Player (و Quiz بعد 25% من الفيديو)، وتسليم الواجبات، والفيدباك |
| الـ Community (`/community`) | أي يوزر مسجّل دخول | Feed، وبوستات بالصور والـ Keywords، وكومنتات وردود، و Reactions |

## التشغيل
اتبع [`backend/README.md`](backend/README.md) و [`frontend/README.md`](frontend/README.md).

**الأدمن الافتراضي:** `Admin@gmail.com` / `Admin@123`

## الجودة
- **API:** 72 Integration test و 3 Unit tests، والـ Build من غير أي Warning.
- **Frontend:** Unit tests بـ Vitest.
- **E2E:** 7 سيناريوهات على المتصفح، والـ API الحقيقية، و SQL Server.

## الخطة التانية
التفاصيل في `docs/SECOND_PLAN.md`.
- **اتعمل (المسار أ):**
  - منع الشراء والتسجيل المكرر.
  - شيل الضريبة من العرض.
  - المحاضرين بس في الجداول.
  - ربط المحاضر بقسم.
  - حماية الأدمن.
  - تعديل الفيديو، وحذف وترتيب أقسام المنهج.
  - شروط أسئلة الامتحان.
- **الجاي:**
  - تسليم الامتحانات وتصحيحها، وحساب الـ Progress.
  - Repost و Share.
  - الإيميلات و Forgot password، و Webhooks.
  - أدوات الأدمن (Events و Rooms وتعارض الجداول).
