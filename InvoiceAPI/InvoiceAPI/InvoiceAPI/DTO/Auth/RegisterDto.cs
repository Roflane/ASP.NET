using InvoiceAPI.Interfaces;

namespace InvoiceAPI.DTO;

public record RegisterDto(
    string Email,
    string Password,
    string Name,
    string? Address
);