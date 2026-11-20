using BFCAI.Application.Features.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService service) : ControllerBase
{
    [HttpPost("login")]
    public Task<AuthResponse> Login(LoginRequest request) => service.LoginAsync(request);

    /// <summary>Visitor registration - creates a Student and signs in.</summary>
    [HttpPost("register")]
    public Task<AuthResponse> Register(RegisterRequest request) => service.RegisterAsync(request);

    /// <summary>"Teach in BFCAI" - creates an Instructor (no sign in).</summary>
    [HttpPost("register-instructor")]
    public async Task<IActionResult> RegisterInstructor(RegisterInstructorRequest request)
    {
        await service.RegisterInstructorAsync(request);
        return NoContent();
    }

    [HttpPost("refresh")]
    public Task<AuthResponse> Refresh(RefreshTokenRequest request) => service.RefreshAsync(request);

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshTokenRequest request)
    {
        await service.LogoutAsync(request);
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public Task<CurrentUserDto> Me() => service.GetCurrentUserAsync();
}
