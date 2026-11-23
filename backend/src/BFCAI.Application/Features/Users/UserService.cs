using BFCAI.Application.Common;
using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Domain.Constants;
using BFCAI.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace BFCAI.Application.Features.Users;

/// <summary>Admin user management (legacy Admin/UserController + admin use of the Register page).</summary>
public interface IUserService
{
    Task<List<UserListItemDto>> GetAllAsync(string? userType);
    Task<UserDto> GetAsync(string id);
    Task<UserDto> CreateAsync(UserRequest request);
    Task<UserDto> UpdateAsync(string id, UserRequest request);
    Task LockAsync(string id);
    Task UnlockAsync(string id);
    Task DeleteAsync(string id);
    Task<List<string>> GetRolesAsync();
}

/// <remarks>
/// Business rules (second plan): an admin can't delete or lock their own account, and the last
/// admin can't lose the Admin role, so the system is never left without an admin.
/// </remarks>
public class UserService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IUserService
{
    public async Task<List<UserListItemDto>> GetAllAsync(string? userType)
    {
        var rows = await unitOfWork.Repository<ApplicationUser>().SelectAsync(
            u => new { u.Id, u.UserName, u.Email, u.FullName, u.UserType, u.LockoutEnabled, u.LockoutEnd },
            string.IsNullOrEmpty(userType) ? null : u => u.UserType == userType);

        // Same check as UserManager.IsLockedOutAsync
        return rows.Select(u => new UserListItemDto(u.Id, u.UserName ?? "", u.Email ?? "", u.FullName, u.UserType,
                u.LockoutEnabled && u.LockoutEnd > DateTimeOffset.UtcNow))
            .ToList();
    }

    public async Task<UserDto> GetAsync(string id)
    {
        var user = await FindAsync(id);
        return await ToDtoAsync(user);
    }

    public async Task<UserDto> CreateAsync(UserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ValidationException([new ValidationFailure(nameof(request.Password), "The Password field is required.")]);

        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FullName = request.FullName ?? string.Empty,
            UserType = request.Role, // fix: legacy Upsert left it empty
            DepartmentId = await CheckDepartmentAsync(request.DepartmentId),
        };

        (await userManager.CreateAsync(user, request.Password)).ThrowIfFailed(nameof(request.Password));
        (await userManager.AddToRoleAsync(user, request.Role)).ThrowIfFailed(nameof(request.Role));

        return await ToDtoAsync(user);
    }

    public async Task<UserDto> UpdateAsync(string id, UserRequest request)
    {
        var user = await FindAsync(id);
        var currentRoles = await userManager.GetRolesAsync(user);

        if (currentRoles.Contains(Roles.Admin) && request.Role != Roles.Admin && await IsLastAdminAsync())
            throw new BadRequestException("This is the only admin, so the Admin role can't be removed from it.");

        user.UserName = request.UserName;
        user.Email = request.Email;
        if (request.FullName is not null) user.FullName = request.FullName;
        user.UserType = request.Role; // fix: keep UserType in sync with the role
        user.DepartmentId = await CheckDepartmentAsync(request.DepartmentId);
        (await userManager.UpdateAsync(user)).ThrowIfFailed();

        // Legacy: if the role changed, remove all roles and add the new one.
        if (!currentRoles.Contains(request.Role))
        {
            (await userManager.RemoveFromRolesAsync(user, currentRoles)).ThrowIfFailed();
            (await userManager.AddToRoleAsync(user, request.Role)).ThrowIfFailed(nameof(request.Role));
        }

        return await ToDtoAsync(user);
    }

    public async Task LockAsync(string id)
    {
        if (id == currentUser.UserId)
            throw new BadRequestException("You can't lock your own account.");

        var user = await FindAsync(id);
        (await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100))).ThrowIfFailed();
    }

    public async Task UnlockAsync(string id)
    {
        var user = await FindAsync(id);
        (await userManager.SetLockoutEndDateAsync(user, null)).ThrowIfFailed();
    }

    public async Task DeleteAsync(string id)
    {
        if (id == currentUser.UserId)
            throw new BadRequestException("You can't delete your own account.");

        var user = await FindAsync(id);
        (await userManager.DeleteAsync(user)).ThrowIfFailed();
    }

    public Task<List<string>> GetRolesAsync()
        => unitOfWork.Repository<IdentityRole>().SelectAsync(r => r.Name!, orderBy: q => q.OrderBy(r => r.Id));

    private async Task<bool> IsLastAdminAsync()
        => (await userManager.GetUsersInRoleAsync(Roles.Admin)).Count <= 1;

    private async Task<int?> CheckDepartmentAsync(int? departmentId)
    {
        if (departmentId is null) return null;
        if (!await unitOfWork.Repository<Department>().AnyAsync(d => d.DepartmentID == departmentId))
            throw new ValidationException([new ValidationFailure(nameof(UserRequest.DepartmentId), "Please select a valid department.")]);
        return departmentId;
    }

    private async Task<ApplicationUser> FindAsync(string id)
        => await userManager.FindByIdAsync(id) ?? throw new NotFoundException("User", id);

    private async Task<UserDto> ToDtoAsync(ApplicationUser user)
    {
        var role = (await userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty;
        return new UserDto(user.Id, user.UserName ?? "", user.Email ?? "", user.FullName, role, user.DepartmentId);
    }
}
