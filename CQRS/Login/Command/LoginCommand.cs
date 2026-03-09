using ASP_NET_22._TaskFlow_CQRS.Application.DTOs;
using MediatR;

namespace ASP_NET_22._TaskFlow_CQRS.Application.Commands.Login;

public record LoginCommand(LoginRequestDto LoginRequestDto) : IRequest<AuthResponseDto>;