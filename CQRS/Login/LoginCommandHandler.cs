using ASP_NET_22._TaskFlow_CQRS.Application.DTOs;
using ASP_NET_22._TaskFlow_CQRS.Application.Interfaces;
using ASP_NET_22._TaskFlow_CQRS.Application.RefreshToken;
using AutoMapper;
using MediatR;

namespace ASP_NET_22._TaskFlow_CQRS.Application.Commands.Login;

public class LoginCommandHandler(IAuthUserStore authUserStore, RefreshTokenImpl refreshTokenImpl) : IRequestHandler<LoginCommand, AuthResponseDto> {
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken) {
        var userId = await authUserStore.FindUserIdByEmailOrIdAsync(request.LoginRequestDto.Email);
        if (userId is null)
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (!await authUserStore.CheckPasswordAsync(userId, request.LoginRequestDto.Password))
            throw new UnauthorizedAccessException("Invalid email or password.");

        return await refreshTokenImpl.GenerateTokensAsync(userId, request.LoginRequestDto.Email);
    }
}