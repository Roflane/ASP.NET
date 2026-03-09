using ASP_NET_22._TaskFlow_CQRS.Application.DTOs;
using ASP_NET_22._TaskFlow_CQRS.Application.Interfaces;
using ASP_NET_22._TaskFlow_CQRS.Application.RefreshToken;
using MediatR;

namespace ASP_NET_22._TaskFlow_CQRS.Application.Commands.Login;

public class RegisterCommandHandler(RefreshTokenImpl refreshTokenImpl, IAuthUserStore authUserStore) : IRequestHandler<RegisterCommand, AuthResponseDto> {
    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken) {
        if (await authUserStore.FindUserIdByEmailOrIdAsync(request.RegisterRequestDto.Email) is not null)
            throw new InvalidOperationException("User with this email already exists.");

        var userId = await authUserStore.CreateUserAsync(request.RegisterRequestDto);
        await authUserStore.AddToRoleAsync(userId, "User");

        return await refreshTokenImpl.GenerateTokensAsync(userId, request.RegisterRequestDto.Email);
    }
}