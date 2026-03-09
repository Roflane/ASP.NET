using FluentValidation;

namespace ASP_NET_22._TaskFlow_CQRS.Application.Commands.Login;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand> {
    public RefreshTokenCommandValidator() {
        RuleFor(x => x.RefreshTokenRequest.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required")
            .NotNull().WithMessage("Refresh token cannot be null")
            .MinimumLength(32)
            .WithMessage("Refresh token must be at least 32 characters long") 
            .MaximumLength(512).WithMessage("Refresh token must not exceed 512 characters");
    }
}