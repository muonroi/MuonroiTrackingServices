using Basket.API.Extensions;
using Common.Logging;
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

    //add services to the container
    builder.Services.ConfigureRedis(builder.Configuration);
    builder.Services.ConfigureServices();

    var app = builder.Build();

    app.UseInfrastructure();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information("Shut down Basket API complete");
    Log.CloseAndFlush();
}