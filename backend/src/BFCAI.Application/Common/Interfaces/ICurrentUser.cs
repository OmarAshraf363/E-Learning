namespace BFCAI.Application.Common.Interfaces;

/// <summary>The authenticated user of the current request (read from the JWT).</summary>
public interface ICurrentUser
{
    string? UserId { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);

    /// <summary>Returns the user id or throws 401 when the request is anonymous.</summary>
    string RequiredUserId { get; }
}
