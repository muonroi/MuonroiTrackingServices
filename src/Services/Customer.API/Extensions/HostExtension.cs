using Microsoft.EntityFrameworkCore;

namespace Customer.API.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();
            try
            {
                logger.LogInformation("Migrating postgresql database.");
                if (context is null) return host;
                ExecuteMigrations(context);
                logger.LogInformation("Migrated postgresql database.");
                InvokeSeeder(seeder, context, services);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while migrating the postgresql database");
            }

            return host;
        }

        private static void ExecuteMigrations<TContext>(TContext context)
        where TContext : DbContext
        {
            context.Database.Migrate();
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
            IServiceProvider services)
            where TContext : DbContext
        {
            seeder(context, services);
        }
    }
}
