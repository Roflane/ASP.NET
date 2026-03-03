namespace InvoiceAPI.Extensions;

public static class AppExtension {
    public static WebApplication BuildExt(this WebApplication app) {
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();

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
        return app;
    }
}