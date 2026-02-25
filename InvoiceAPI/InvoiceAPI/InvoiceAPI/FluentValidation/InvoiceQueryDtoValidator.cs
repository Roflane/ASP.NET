using FluentValidation;
using InvoiceAPI.DTO;

namespace InvoiceAPI.FluentValidation;

public class InvoiceQueryDtoValidator : AbstractValidator<InvoiceQueryDto> {
    private static readonly string[] AllowedSortFields = {
        "createdat",
        "updatedat",
        "startdate",
        "enddate",
        "status"
    };

    public InvoiceQueryDtoValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.SortBy)
            .Must(x => AllowedSortFields.Contains(x.ToLower()))
            .WithMessage("Invalid sort field");

        RuleFor(x => x.SortDirection)
            .Must(x => x is "asc" or "desc");

        RuleFor(x => x)
            .Must(x => x.DateFrom == null || x.DateTo == null || x.DateFrom <= x.DateTo)
            .WithMessage("DateFrom must be less or equal DateTo");
    }
}