using System.Security.Claims;
using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;

namespace BFCAI.Api.Infrastructure;

public class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal => accessor.HttpContext?.User;

    public string? UserId => Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;
    public bool IsInRole(string role) => Principal?.IsInRole(role) ?? false;
    public string RequiredUserId => UserId ?? throw new UnauthorizedException();
}
