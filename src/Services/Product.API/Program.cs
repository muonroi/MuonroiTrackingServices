using Common.Logging;
using Product.API.Extensions;
using Product.API.Persistence;
using Serilog;
var builder = WebApplication.CreateBuilder(args);
Log.Information("Starting CatalogProduct API up");
try
{
    builder.Host.UseSerilog(SerilogAction.Configure);

    builder.Host.AddConfigurationHost();

    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    app.UseInfrastructure();

    app.MigrateDatabase<ProductContext>((context, _) =>
        {
            ProductContextSeed.SeedProductAsync(context, Log.Logger).Wait();
        })
        .Run();

    app.Run();
}
catch (Exception ex)
{
    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "CatalogProduct API terminated unexpectedly");
}
finally
{
    Log.Information("Shut down CatalogProduct API complete");
    Log.CloseAndFlush();
}

