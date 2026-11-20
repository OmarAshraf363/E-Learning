using BFCAI.Domain.Entities;

namespace BFCAI.Application.Common.Interfaces;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) CreateAccessToken(ApplicationUser user, IList<string> roles);

    /// <summary>Creates a random refresh token; returns the raw value and its stored hash.</summary>
    (string Token, string Hash, DateTime ExpiresAt) CreateRefreshToken();

    string Hash(string refreshToken);
}
