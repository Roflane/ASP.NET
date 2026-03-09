using FluentValidation;

namespace ASP_NET_22._TaskFlow_CQRS.Application.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand> {
    public LoginCommandValidator() {
        RuleFor(x => x.LoginRequestDto.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters");

        RuleFor(x => x.LoginRequestDto.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters");
    }
}
