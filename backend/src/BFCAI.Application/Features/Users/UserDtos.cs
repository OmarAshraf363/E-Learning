namespace BFCAI.Application.Features.Users;

/// <summary>Row of the admin "Manage Students / Instructors" table.</summary>
public record UserListItemDto(string Id, string UserName, string Email, string FullName, string UserType, bool IsLockedOut);

/// <summary>Legacy UserViewModel (+ FullName, which the legacy admin Register page also asked for).</summary>
public record UserDto(string Id, string UserName, string Email, string FullName, string Role);

/// <summary>Create (Password required) or edit (Password ignored) a user.</summary>
public record UserRequest(string UserName, string Email, string Role, string? FullName, string? Password);
