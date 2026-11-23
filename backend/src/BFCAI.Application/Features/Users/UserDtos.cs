namespace BFCAI.Application.Features.Users;

/// <summary>Row of the admin "Manage Students / Instructors" table.</summary>
public record UserListItemDto(string Id, string UserName, string Email, string FullName, string UserType, bool IsLockedOut);

/// <summary>Legacy UserViewModel (+ FullName, which the legacy admin Register page also asked for).</summary>
public record UserDto(string Id, string UserName, string Email, string FullName, string Role, int? DepartmentId);

/// <summary>
/// Create (Password required) or edit (Password ignored) a user.
/// DepartmentId links an instructor to a department: the instructor then creates courses in that
/// department only (without a department the instructor can pick any department).
/// </summary>
public record UserRequest(string UserName, string Email, string Role, string? FullName, string? Password, int? DepartmentId = null);
