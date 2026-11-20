using BFCAI.Application.Features.Users;
using BFCAI.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Admin;

/// <summary>Legacy Admin/UserController.</summary>
[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = Roles.Admin)]
public class UsersController(IUserService service) : ControllerBase
{
    /// <summary>?filter=Student | Instructor (matches ApplicationUser.UserType, like the legacy page).</summary>
    [HttpGet]
    public Task<List<UserListItemDto>> GetAll([FromQuery] string? filter) => service.GetAllAsync(filter);

    [HttpGet("roles")]
    public Task<List<string>> GetRoles() => service.GetRolesAsync();

    [HttpGet("{id}")]
    public Task<UserDto> Get(string id) => service.GetAsync(id);

    [HttpPost]
    public Task<UserDto> Create(UserRequest request) => service.CreateAsync(request);

    [HttpPut("{id}")]
    public Task<UserDto> Update(string id, UserRequest request) => service.UpdateAsync(id, request);

    [HttpPost("{id}/lock")]
    public async Task<IActionResult> Lock(string id)
    {
        await service.LockAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/unlock")]
    public async Task<IActionResult> Unlock(string id)
    {
        await service.UnlockAsync(id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}
