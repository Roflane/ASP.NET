using FluentValidation;

namespace ASP_NET_22._TaskFlow_CQRS.Application.Commands.Login;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand> {
    public RegisterCommandValidator() {
        RuleFor(x => x.RegisterRequestDto.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters");
        
        RuleFor(x => x.RegisterRequestDto.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters");
        
        RuleFor(x => x.RegisterRequestDto.ConfirmPassword)
            .NotEmpty().WithMessage("Confirmed password is required")
            .Equal(x => x.RegisterRequestDto.Password).WithMessage("Passwords do not match");
        
        RuleFor(x => x.RegisterRequestDto.FirstName)
            .NotEmpty().WithMessage("Firstname is required")
            .MinimumLength(2).WithMessage("Firstname must be at least 2 characters long");
        
        RuleFor(x => x.RegisterRequestDto.LastName)
            .NotEmpty().WithMessage("Lastname is required")
            .MinimumLength(2).WithMessage("Lastname must be at least 2 characters long");
    }
}