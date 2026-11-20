# Architecture

```
BFCAI.Api  ──►  BFCAI.Infrastructure  ──►  BFCAI.Application  ──►  BFCAI.Domain
(HTTP)          (EF, Identity, Redis,        (Services, DTOs,         (Entities,
                 Stripe, Files)               Interfaces, Validators)  Constants)
```

السهم معناه "بيعتمد على". **الـ Application ما يعرفش أي حاجة عن EF Core ولا Stripe ولا Redis**: بيتعامل معاهم من خلال Interfaces بس.

## كل طبقة وقواعدها

| الطبقة | فيها إيه | ممنوع فيها |
|---|---|---|
| **Domain** | Entities (نفس الجداول بالظبط)، Enums، Constants (`Roles`, `PaymentStatus`) | أي لوجيك بيكلّم الداتابيز أو HTTP |
| **Application** | `Features/<Feature>/` فيها Service + DTOs + Validators، وفي `Common/` الـ Interfaces والـ Exceptions والـ Pagination | `DbContext` و `IFormFile` و `HttpContext` |
| **Infrastructure** | `AppDbContext`، Configurations، Migrations، `GenericRepository`، `UnitOfWork`، JWT، تخزين الملفات، Redis، Stripe | Business rules |
| **Api** | Controllers رفيعة، Program.cs، Exception handler، Validation filter | Business logic |

## مسار الـ Request

```
HTTP ─► Controller ─► ValidationFilter (FluentValidation) ─► Service ─► IUnitOfWork / Repositories ─► DB
                                                              │
                                     throws NotFound/Forbidden/BadRequest ─► GlobalExceptionHandler ─► ProblemDetails
```

### Controller
بيعمل حاجة واحدة: ياخد الـ Request ويستدعي الـ Service ويرجّع النتيجة.
```csharp
[HttpGet("{id:int}")]
public Task<DepartmentDto> Get(int id) => service.GetAsync(id);
```

### Service (البيزنس لوجيك كله هنا)
- بتاخد `IUnitOfWork` للداتا و `ICurrentUser` لليوزر الحالي (من التوكن).
- لو حصل خطأ بترمي Exception (`NotFoundException`، `ForbiddenException`، `BadRequestException`، `UnauthorizedException`)، وبعدين بتتحوّل لـ ProblemDetails بالـ Status المناسب.
- **`UserId` بيتاخد دايمًا من `ICurrentUser`، عمره ما يتاخد من الـ Body.**

### Data access
- `unitOfWork.Repository<T>()` فيه كل العمليات العامة: `GetOneAsync` و `GetAllAsync` و `SelectAsync` (Projection لـ DTO) و `GetPagedAsync` و `AnyAsync` و `CountAsync` و `Add` و `Update` و `Delete`.
- القراءة **من غير Tracking** بشكل افتراضي. لو هتعدّل الـ Entity، ابعت `tracked: true`.
- مفيش حاجة بتتحفظ غير لما تنادي `await unitOfWork.SaveChangesAsync()`، **مرة واحدة في آخر العملية**.
- الاستعلامات المعقّدة (Projections كبيرة فيها Nested data) بتتكتب في Repository خاص: Interface في `Application/Features/<X>/` والـ Implementation في `Infrastructure/Persistence/Repositories/`.

### Mapping
يدوي، من غير مكتبة: `Select(x => new XDto(...))` جوه الاستعلام (Projection)، أو Extension method `ToDto()` في نفس فولدر الـ Feature.

### Validation
أي `AbstractValidator<TRequest>` في الـ Application بيتسجّل ويتنفّذ لوحده قبل الـ Action. الخطأ بيرجع 400 بالشكل ده:
```json
{ "status": 400, "title": "Validation failed", "errors": { "Name": ["'Name' must not be empty."] } }
```

### Auth
- JWT Access Token صلاحيته 15 دقيقة، و Refresh Token صلاحيته 7 أيام. الـ Refresh Token متخزن Hash في جدول `RefreshTokens`.
- الأدوار: `Admin` و `Instructor` و `Student`. الـ Controllers بتستخدم `[Authorize(Roles = Roles.Admin)]`.

### الملفات
`IFileStorage` بيحفظ تحت `Storage:RootPath`، والافتراضي `wwwroot` بتاع الـ API، والملفات بتتعرض على نفس المسار (مثلًا `/Covers/x.png`). امتدادات الملفات المسموحة موجودة في `FileRules`.

## الداتابيز
- `Persistence/Migrations/*_Baseline`: نفس Schema الـ MVC بالظبط.
- `*_AddRefreshTokens`: أول إضافة جديدة.
- Migration جديدة:
  ```bash
  dotnet ef migrations add <Name> --project src/BFCAI.Infrastructure --startup-project src/BFCAI.Api -o Persistence/Migrations
  ```
