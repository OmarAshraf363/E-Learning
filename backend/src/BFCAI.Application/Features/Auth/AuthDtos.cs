namespace BFCAI.Application.Features.Auth;

/// <summary>Legacy login form field is "Email" but the value is matched as user name first (same as PasswordSignInAsync).</summary>
public record LoginRequest(string Email, string Password);

/// <summary>Public registration (legacy Identity Register page) - always creates a Student.</summary>
public record RegisterRequest(string FullName, string Email, string Password, string ConfirmPassword);

/// <summary>"Teach in BFCAI" form on the home page (legacy HomeController.PostRegister).</summary>
public record RegisterInstructorRequest(string FullName, string Email, string Password, string ConfirmPassword);

public record RefreshTokenRequest(string RefreshToken);

public record CurrentUserDto(
    string Id,
    string UserName,
    string Email,
    string FullName,
    string? Picture,
    IReadOnlyList<string> Roles);

public record AuthResponse(
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    string RefreshToken,
    CurrentUserDto User);
