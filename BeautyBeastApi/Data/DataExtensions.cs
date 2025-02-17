using Microsoft.EntityFrameworkCore;

namespace BeautyBeastApi.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BeautyBeastContext>();
        dbContext.Database.Migrate();
    }
}