# إضافة Feature جديدة (خطوة بخطوة)

المثال: CRUD بسيط للأقسام (Departments). أي Feature تانية بتمشي على نفس الخطوات.

## 1) الـ DTOs: `Application/Features/Departments/DepartmentDtos.cs`
```csharp
namespace BFCAI.Application.Features.Departments;

public record DepartmentDto(int Id, string Name, string Description, string? Cover);

public record DepartmentRequest(string DepartmentName, string DepartmentDescription);
```

## 2) الـ Validator: `Application/Features/Departments/DepartmentRequestValidator.cs`
بيتنفّذ لوحده قبل الـ Controller.
```csharp
public class DepartmentRequestValidator : AbstractValidator<DepartmentRequest>
{
    public DepartmentRequestValidator()
    {
        RuleFor(x => x.DepartmentName).NotEmpty();
    }
}
```

## 3) الـ Service: `Application/Features/Departments/DepartmentService.cs`
```csharp
public interface IDepartmentService
{
    Task<List<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto> GetAsync(int id);
    Task<DepartmentDto> CreateAsync(DepartmentRequest request);
    Task<DepartmentDto> UpdateAsync(int id, DepartmentRequest request);
    Task DeleteAsync(int id);
}

public class DepartmentService(IUnitOfWork unitOfWork) : IDepartmentService
{
    private IGenericRepository<Department> Departments => unitOfWork.Repository<Department>();

    public Task<List<DepartmentDto>> GetAllAsync()
        => Departments.SelectAsync(d => new DepartmentDto(d.DepartmentID, d.DepartmentName, d.DepartmentDescription, d.Cover));

    public async Task<DepartmentDto> GetAsync(int id)
        => await Departments.SelectOneAsync(d => d.DepartmentID == id,
               d => new DepartmentDto(d.DepartmentID, d.DepartmentName, d.DepartmentDescription, d.Cover))
           ?? throw new NotFoundException(nameof(Department), id);

    public async Task<DepartmentDto> UpdateAsync(int id, DepartmentRequest request)
    {
        var entity = await Departments.GetOneAsync(d => d.DepartmentID == id, tracked: true)
                     ?? throw new NotFoundException(nameof(Department), id);
        entity.DepartmentName = request.DepartmentName;
        entity.DepartmentDescription = request.DepartmentDescription;
        await unitOfWork.SaveChangesAsync();
        return await GetAsync(id);
    }
    // ... Create / Delete بنفس الطريقة
}
```
القواعد:
- اليوزر الحالي بيتاخد من `ICurrentUser` (حقنه في الـ Constructor).
- لو في مشكلة: `throw new NotFoundException(...)` أو `ForbiddenException` أو `BadRequestException`.
- `SaveChangesAsync()` مرة واحدة في الآخر.

## 4) التسجيل في الـ DI: `Application/DependencyInjection.cs`
```csharp
services.AddScoped<IDepartmentService, DepartmentService>();
```

## 5) الـ Controller: `Api/Controllers/Admin/DepartmentsController.cs`
```csharp
[ApiController]
[Route("api/admin/departments")]
[Authorize(Roles = Roles.Admin)]
public class DepartmentsController(IDepartmentService service) : ControllerBase
{
    [HttpGet]               public Task<List<DepartmentDto>> GetAll() => service.GetAllAsync();
    [HttpGet("{id:int}")]   public Task<DepartmentDto> Get(int id) => service.GetAsync(id);
    [HttpPost]              public Task<DepartmentDto> Create(DepartmentRequest request) => service.CreateAsync(request);
    [HttpPut("{id:int}")]   public Task<DepartmentDto> Update(int id, DepartmentRequest request) => service.UpdateAsync(id, request);
    [HttpDelete("{id:int}")] public Task Delete(int id) => service.DeleteAsync(id);
}
```
**لو فيه رفع ملفات:** استقبل Form record فيه `IFormFile` (`[FromForm]`) في الـ Controller، وحوّله لـ Request الخاص بالـ Application باستخدام `file.ToFileUpload()`. بعد كده شغّل الـ Validator بنفسك، لأن الـ ValidationFilter بيتحقق من الـ Arguments اللي جاية للـ Action بس. المثال من `AccountController`:
```csharp
public record UpdateProfileForm(string? PhoneNumber, string? JopDescription, IFormFile? ProfilePicture);

[HttpPut("profile")]
public async Task<ProfileDto> UpdateProfile([FromForm] UpdateProfileForm form,
    [FromServices] IValidator<UpdateProfileRequest> validator)
{
    var request = new UpdateProfileRequest(form.PhoneNumber, form.JopDescription, form.ProfilePicture.ToFileUpload());
    await validator.ValidateAndThrowAsync(request);
    return await service.UpdateProfileAsync(request);
}
```

## 6) Test: `tests/BFCAI.Application.Tests/Departments/DepartmentServiceTests.cs`
```csharp
[Fact]
public async Task Update_changes_name()
{
    using var db = new TestDb();                       // SQLite in-memory + seed data
    var service = new DepartmentService(db.UnitOfWork);

    var result = await service.UpdateAsync(1, new DepartmentRequest("CS", "desc"));

    Assert.Equal("CS", result.Name);
}
```

## Checklist
- [ ] DTOs و Validator و Service جوه `Application/Features/<Feature>/`
- [ ] سطر التسجيل في `Application/DependencyInjection.cs`
- [ ] Controller عليه `[Authorize(Roles = ...)]`
- [ ] Tests للوجيك المهم
- [ ] لو الـ Entity اتغيّرت: Migration جديدة
