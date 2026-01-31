using InvoiceAPI.Db;
using InvoiceAPI.Interfaces;
using InvoiceAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Invoice API",
        Description = "API for both customer and invoice"
    });
});

// Db
var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddDbContext<InvoiceAPIContext>(options =>
    options.UseSqlServer(connectionString)
);

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// redirect "/" → swagger
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();