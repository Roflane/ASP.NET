using ASP_NET_22._TaskFlow_CQRS.Application.DTOs;
using ASP_NET_22._TaskFlow_CQRS.Application.Interfaces;
using ASP_NET_22._TaskFlow_CQRS.Application.RefreshToken;
using MediatR;

namespace ASP_NET_22._TaskFlow_CQRS.Application.Commands.Login;

public class RefreshTokenCommandHandler(
    RefreshTokenImpl refreshTokenImpl,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtTokenService jwtTokenService,
    IAuthUserStore authUserStore) : IRequestHandler<RefreshTokenCommand, AuthResponseDto> {
    
    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken) {
        var (userId, jti) = jwtTokenService.ValidateRefreshTokenAndGetJti(request.RefreshTokenRequest.RefreshToken);

        var storedToken = await refreshTokenRepository.GetByJwtIdAsync(jti);
        if (storedToken is null)
            throw new UnauthorizedAccessException("Invalid refresh token");
        if (!storedToken.IsActive)
            throw new UnauthorizedAccessException("Refresh token has been revoked or expired");

        storedToken.RevokedAt = DateTime.UtcNow;

        var email = await authUserStore.GetEmailAsync(userId);
        var newTokens = await refreshTokenImpl.GenerateTokensAsync(userId, email);
        var newJti = jwtTokenService.GetJtiFromRefreshToken(newTokens.RefreshToken);
        var newStoredToken = string.IsNullOrEmpty(newJti) ? null : await refreshTokenRepository.GetByJwtIdAsync(newJti);
        if (newStoredToken is not null)
            storedToken.ReplacedByJwtId = newStoredToken.JwtId;

        await refreshTokenRepository.UpdateAsync(storedToken);
        return newTokens;
    }
}