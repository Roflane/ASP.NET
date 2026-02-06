using FluentValidation;
using InvoiceAPI.DTO;

namespace InvoiceAPI.FluentValidation;

public class InvoiceRowDtoValidator : AbstractValidator<InvoiceRowDto>
{
    public InvoiceRowDtoValidator()
    {
        RuleFor(x => x.Service)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Quantity)
            .GreaterThan(0);

        RuleFor(x => x.Rate)
            .GreaterThan(0);
    }
}