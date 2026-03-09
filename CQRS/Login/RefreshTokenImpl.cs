using ASP_NET_22._TaskFlow_CQRS.Application.DTOs;
using ASP_NET_22._TaskFlow_CQRS.Application.Interfaces;

namespace ASP_NET_22._TaskFlow_CQRS.Application.RefreshToken;

public class RefreshTokenImpl(IRefreshTokenRepository refreshTokenRepository, IJwtTokenService jwtTokenService, IAuthUserStore authUserStore) {
    public async Task<AuthResponseDto> GenerateTokensAsync(string userId, string? email) {
        var roles = await authUserStore.GetRolesAsync(userId);
        var (accessToken, expiresAt) = await jwtTokenService.GenerateAccessTokenAsync(userId, email ?? "", roles);
        var (refreshEntity, refreshJwt) = await jwtTokenService.CreateRefreshTokenAsync(userId);

        return new AuthResponseDto {
            AccessToken = accessToken,
            ExpiresAt = expiresAt,
            RefreshToken = refreshJwt,
            RefreshTokenExpiresAt = refreshEntity.ExpiresAt,
            Email = email ?? "",
            Roles = roles
        };
    }
    
    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest) {
        var (userId, jti) = jwtTokenService.ValidateRefreshTokenAndGetJti(refreshTokenRequest.RefreshToken);

        var storedToken = await refreshTokenRepository.GetByJwtIdAsync(jti);
        if (storedToken is null)
            throw new UnauthorizedAccessException("Invalid refresh token");
        if (!storedToken.IsActive)
            throw new UnauthorizedAccessException("Refresh token has been revoked or expired");

        storedToken.RevokedAt = DateTime.UtcNow;

        var email = await authUserStore.GetEmailAsync(userId);
        var newTokens = await GenerateTokensAsync(userId, email);
        var newJti = jwtTokenService.GetJtiFromRefreshToken(newTokens.RefreshToken);
        var newStoredToken = string.IsNullOrEmpty(newJti) ? null : await refreshTokenRepository.GetByJwtIdAsync(newJti);
        if (newStoredToken is not null)
            storedToken.ReplacedByJwtId = newStoredToken.JwtId;

        await refreshTokenRepository.UpdateAsync(storedToken);
        return newTokens;
    }
}