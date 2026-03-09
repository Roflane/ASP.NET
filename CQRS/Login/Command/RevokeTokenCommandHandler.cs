using ASP_NET_22._TaskFlow_CQRS.Application.Interfaces;
using MediatR;

namespace ASP_NET_22._TaskFlow_CQRS.Application.Commands.Login;

public class RevokeTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, IJwtTokenService jwtTokenService) : IRequestHandler<RevokeTokenCommand, bool> {
    public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken) {
        string jti = "";
        try {
            jwtTokenService.ValidateRefreshTokenAndGetJti(
                request.RefreshTokenRequest.RefreshToken, 
                validateLifetime: false);
        }
        catch (Exception) {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        var storedToken = await refreshTokenRepository.GetByJwtIdAsync(jti);
        if (storedToken is null || !storedToken.IsActive) return false;

        storedToken.RevokedAt = DateTime.UtcNow;
        await refreshTokenRepository.UpdateAsync(storedToken);
        return true;
    }
}