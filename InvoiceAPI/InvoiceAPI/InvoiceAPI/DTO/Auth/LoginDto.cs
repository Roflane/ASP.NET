using InvoiceAPI.Interfaces;

namespace InvoiceAPI.DTO;

public record LoginDto(
    string Email,
    string Password
);