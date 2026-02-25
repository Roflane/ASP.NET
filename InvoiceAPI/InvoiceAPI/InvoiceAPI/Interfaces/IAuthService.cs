using InvoiceAPI.DTO;
using InvoiceAPI.Models;

namespace InvoiceAPI.Interfaces;

public interface IAuthService {
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByEmailAsync(string email);
}