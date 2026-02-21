using InvoiceAPI.Db;
using InvoiceAPI.DTO;
using InvoiceAPI.Interfaces;
using InvoiceAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InvoiceAPI.Services;

public class AuthService : IAuthService {
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly InvoiceAPIContext _context;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration,
        InvoiceAPIContext context) {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto) {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null) {
            throw new InvalidOperationException("User with that email already exists.");
        }

        var user = new User {
            Email = registerDto.Email,
            UserName = registerDto.Email,
            Name = registerDto.Name,
            Address = registerDto.Address,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create user: {errors}");
        }

        var token = await GenerateJwtTokenAsync(user);

        return new AuthResponseDto(user.Email, user.Name, token);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto) {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null) {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: false);

        if (!result.Succeeded) {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        user.UpdatedAt = DateTimeOffset.UtcNow;
        await _userManager.UpdateAsync(user);

        var token = await GenerateJwtTokenAsync(user);

        return new AuthResponseDto(user.Email, user.Name, token);
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword) {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) {
            return false;
        }

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        
        if (result.Succeeded) {
            user.UpdatedAt = DateTimeOffset.UtcNow;
            await _userManager.UpdateAsync(user);
            return true;
        }

        return false;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId) {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<User?> GetUserByEmailAsync(string email) {
        return await _userManager.FindByEmailAsync(email);
    }

    private async Task<string> GenerateJwtTokenAsync(User user) {
        var roles = await _userManager.GetRolesAsync(user);
        
        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("name", user.Name)
        };

        foreach (var role in roles) {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"] ?? "7"));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}