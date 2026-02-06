using FluentValidation;
using InvoiceAPI.DTO;

namespace InvoiceAPI.FluentValidation;

public class CreateInvoiceDtoValidator : AbstractValidator<CreateInvoiceDto> {
    public CreateInvoiceDtoValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0);

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate);

        RuleFor(x => x.Rows)
            .NotEmpty();

        RuleForEach(x => x.Rows)
            .SetValidator(new InvoiceRowDtoValidator());
    }
}