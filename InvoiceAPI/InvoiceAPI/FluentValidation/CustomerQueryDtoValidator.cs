using FluentValidation;
using InvoiceAPI.DTO;

namespace InvoiceAPI.FluentValidation;

public class CustomerQueryDtoValidator : AbstractValidator<CustomerQueryDto> {
    private static readonly string[] AllowedSortFields = {
        "name",
        "email",
        "createdAt",
        "updatedAt"
    };

    public CustomerQueryDtoValidator() {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.SortBy)
            .Must(x => AllowedSortFields.Contains(x.ToLower()))
            .WithMessage("Invalid sort field");

        RuleFor(x => x.SortDirection)
            .Must(x => x is "asc" or "desc")
            .WithMessage("SortDirection must be 'asc' or 'desc'");
    }
}