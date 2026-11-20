# BFCAI Frontend (Angular 22)

الواجهة الجديدة لمنصة BFCAI. الـ API بتاعها موجودة في `../backend`.

## التشغيل
```bash
npm install
npm start          # http://localhost:4200   (الـ API لازم تكون شغّالة على http://localhost:5151)
npm test           # Vitest
npm run build      # Production build -> dist/
```
عنوان الـ API بيتظبط من `src/environments/environment*.ts`.

## الهيكل
```
src/app/
  core/                 حاجات بتتعمل مرة واحدة للتطبيق كله
    api/                ApiClient (HttpClient wrapper) + BaseApiService<T> (Generic CRUD)
    state/              CrudStore<T> (Generic state بالـ signals)
    auth/               AuthService, interceptor (JWT + refresh), guards (auth / role / guest)
    errors/             error.interceptor (ProblemDetails -> SweetAlert)
    ui/                 NotifyService (SweetAlert2: success / error / confirm)
    models/             PagedResult, ProblemDetails
  shared/               Components / Pipes قابلة لإعادة الاستخدام (pagination, form-errors, fileUrl...)
  layouts/              Layout لكل منطقة: public, admin, instructor, student, community
  features/<feature>/   Feature-based: كل Feature فولدر مستقل وبيتحمّل Lazy
    data-access/        <feature>.api.ts (extends BaseApiService) + <feature>.store.ts (signals)
    pages/              صفحات مربوطة بـ Route (Smart)
    components/         Components للعرض بس، بتتعامل بـ input()/output()
    models/             Interfaces للـ DTOs
    <feature>.routes.ts
  styles/               _variables.scss + legacy/ (الـ CSS القديم بالظبط، ومتقسّم Scope لكل Layout)
```

## المبادئ
- **Standalone + Signals + Zoneless + Control flow الجديد** (`@if` / `@for`).
- **مهم:** في Angular 22 الـ Components بقت **OnPush بشكل افتراضي**. يعني أي State بتظهر في الـ Template لازم تكون `signal()` أو `computed()` أو `input()`؛ لو متغيّر عادي اتغيّر، الشاشة مش هتتحدّث. الـ Forms بتشتغل عادي، و`<app-form-errors>` بيسمع لأحداث الـ Control لوحده.
- الصفحة ما بتكلّمش `HttpClient` مباشرة. الترتيب دايمًا: Page ← Store ← Api service ← ApiClient.
- أي خطأ من الـ API بيظهر لوحده في SweetAlert. أخطاء الـ Validation بتظهر تحت الحقول باستخدام `applyServerErrors` و `<app-form-errors>`.
- الملفات (صور وفيديوهات) بتتعرض بالـ Pipe `fileUrl`.

اقرأ [docs/ADD-A-FEATURE.md](docs/ADD-A-FEATURE.md) قبل ما تضيف صفحة جديدة.
