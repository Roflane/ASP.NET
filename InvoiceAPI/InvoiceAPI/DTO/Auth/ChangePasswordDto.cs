namespace InvoiceAPI.DTO;

public record ChangePasswordDto(
    string CurrentPassword,
    string NewPassword
);