using BFCAI.Application.Common;
using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Domain.Constants;
using BFCAI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BFCAI.Application.Features.Auth;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task RegisterInstructorAsync(RegisterInstructorRequest request);
    Task<AuthResponse> RefreshAsync(RefreshTokenRequest request);
    Task LogoutAsync(RefreshTokenRequest request);
    Task<CurrentUserDto> GetCurrentUserAsync();
}

public class AuthService(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser) : IAuthService
{
    private IGenericRepository<RefreshToken> RefreshTokens => unitOfWork.Repository<RefreshToken>();

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // Legacy: PasswordSignInAsync(Input.Email, ...) -> the value is a user name.
        // Users created by the register pages use the email as user name; admin-created users may not.
        var user = await userManager.FindByNameAsync(request.Email)
                   ?? await userManager.FindByEmailAsync(request.Email);

        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            throw new UnauthorizedException("Invalid login attempt.");

        // Admin "Lock user" sets LockoutEnd (legacy UserController.LockUser).
        if (await userManager.IsLockedOutAsync(user))
            throw new UnauthorizedException("User account locked out.");

        return await IssueTokensAsync(user);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Legacy Register page for visitors: user name = email, role = Student, signs the user in.
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName ?? string.Empty,
            UserType = Roles.Student,
        };

        (await userManager.CreateAsync(user, request.Password)).ThrowIfFailed();
        (await userManager.AddToRoleAsync(user, Roles.Student)).ThrowIfFailed();

        return await IssueTokensAsync(user);
    }

    public async Task RegisterInstructorAsync(RegisterInstructorRequest request)
    {
        // Legacy HomeController.PostRegister: creates an Instructor, does NOT sign in.
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName ?? string.Empty,
            UserType = Roles.Instructor, // fix: legacy left it empty, so the user was missing from admin lists
        };

        (await userManager.CreateAsync(user, request.Password)).ThrowIfFailed();
        (await userManager.AddToRoleAsync(user, Roles.Instructor)).ThrowIfFailed();
    }

    public async Task<AuthResponse> RefreshAsync(RefreshTokenRequest request)
    {
        var hash = tokenService.Hash(request.RefreshToken);
        var stored = await RefreshTokens.GetOneAsync(t => t.TokenHash == hash, tracked: true);

        if (stored is null || !stored.IsActive)
            throw new UnauthorizedException("Invalid refresh token.");

        var user = await userManager.FindByIdAsync(stored.UserId);
        if (user is null || await userManager.IsLockedOutAsync(user))
            throw new UnauthorizedException("Invalid refresh token.");

        // Rotation: every refresh token can be used once.
        stored.RevokedAt = DateTime.UtcNow;
        return await IssueTokensAsync(user);
    }

    public async Task LogoutAsync(RefreshTokenRequest request)
    {
        var hash = tokenService.Hash(request.RefreshToken);
        var stored = await RefreshTokens.GetOneAsync(t => t.TokenHash == hash, tracked: true);
        if (stored is null || stored.RevokedAt is not null) return;

        stored.RevokedAt = DateTime.UtcNow;
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<CurrentUserDto> GetCurrentUserAsync()
    {
        var user = await userManager.FindByIdAsync(currentUser.RequiredUserId)
                   ?? throw new UnauthorizedException();
        return await ToDtoAsync(user);
    }

    private async Task<AuthResponse> IssueTokensAsync(ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var (accessToken, expiresAt) = tokenService.CreateAccessToken(user, roles);
        var (refreshToken, hash, refreshExpiresAt) = tokenService.CreateRefreshToken();

        await RefreshTokens.AddAsync(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = hash,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = refreshExpiresAt,
        });
        await unitOfWork.SaveChangesAsync();

        return new AuthResponse(accessToken, expiresAt, refreshToken, ToDto(user, roles));
    }

    private async Task<CurrentUserDto> ToDtoAsync(ApplicationUser user)
        => ToDto(user, await userManager.GetRolesAsync(user));

    private static CurrentUserDto ToDto(ApplicationUser user, IList<string> roles)
        => new(user.Id, user.UserName ?? string.Empty, user.Email ?? string.Empty, user.FullName, user.Picture, roles.ToList());
}
