using Common.Logging;
using Customer.API.Controller;
using Customer.API.Extensions;
using Customer.API.Persistence;
using Serilog;
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
Log.Information($"Starting {builder.Environment.ApplicationName} API up");
try
{
    builder.Host.UseSerilog(SerilogAction.Configure);

    builder.Host.AddConfigurationHost();

    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();
    app.MapGet("/", () => $"Welcome to {builder.Environment.ApplicationName}!");
    app.MapCustomersApi();
    app.UseInfrastructure();

    app.SeedCustomerData()
        .Run();
}
catch (Exception ex)
{
    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, $"{builder.Environment.ApplicationName} API terminated unexpectedly");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} API complete");
    Log.CloseAndFlush();
}