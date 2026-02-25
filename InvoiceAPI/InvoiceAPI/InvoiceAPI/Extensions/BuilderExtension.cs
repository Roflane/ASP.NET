using System.Reflection;
using System.Text;
using FluentValidation;
using InvoiceAPI.Db;
using InvoiceAPI.Interfaces;
using InvoiceAPI.Models;
using InvoiceAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace InvoiceAPI.Extensions;

public static class BuilderExtension {
    public static WebApplicationBuilder BuildExt(this WebApplicationBuilder builder) {
        builder.Services.AddControllers();
        builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<InvoiceAPIContext>() .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IInvoiceService, InvoiceService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddSwaggerGen(options => {
            options.SwaggerDoc("v1", new OpenApiInfo {
                Version = "v1",
                Title = "Invoice API",
                Description = "API for both customer and invoice"
            });
            
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
        var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
        builder.Services.AddDbContext<InvoiceAPIContext>(options =>
            options.UseSqlServer(connectionString)
        );
        return builder;
    }
}