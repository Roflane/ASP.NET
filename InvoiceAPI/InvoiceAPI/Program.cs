using InvoiceAPI.Extensions;
using PdfSharp.Fonts;

GlobalFontSettings.UseWindowsFontsUnderWindows = true;

var builder = WebApplication.CreateBuilder(args);
builder.BuildExt();
var app = builder.Build();
app.BuildExt();
app.Run();