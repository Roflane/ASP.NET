using ASP_NET_08._TaskFlow_DTOs.Data;
using ASP_NET_08._TaskFlow_DTOs.Services;
using ASP_NET_08._TaskFlow_DTOs.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
   // "TaskFlowDBConnectionString": "Server=(localdb)\\MSSQLLocalDB;Database=TaskFlowDB;Integrated Security=True;Trust Server Certificate=True;"

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var connectionString = builder
    .Configuration
    .GetConnectionString("TaskFlowDBConnectionString");

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskItemService, TaskItemService>();

builder.Services.AddDbContext<TaskFlowDbContext>(
    options =>
        options.UseSqlServer(connectionString)
    );

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseAuthorization();
app.MapControllers();

app.UseHttpsRedirection();
app.Use(async (context, next) => {
    if (context.Request.Path == "/") {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});
app.Run();
