namespace InvoiceAPI.DTO;

public record AuthResponseDto(
    string Email,
    string Name,
    string Token
);