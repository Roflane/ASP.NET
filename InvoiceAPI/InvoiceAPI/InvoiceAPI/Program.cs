using InvoiceAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.BuildExt();

var app = builder.Build();
app.BuildExt();
app.Run();