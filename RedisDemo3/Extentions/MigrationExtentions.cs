using Microsoft.EntityFrameworkCore;
using RedisDemo3.DBContext;

namespace RedisDemo3.Extentions
{
    public static class MigrationExtentions
    {
        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DbContextClass>();
                dbContext.Database.Migrate();
            }
        }
    }
}
