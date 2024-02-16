using Common.Logging;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    Log.Information("Starting Basket API up");

    builder.Host.UseSerilog(SerilogAction.Configure);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Basket API terminated unexpectedly");
}
finally
{
    Log.Information("Shut down Basket API complete");
    Log.CloseAndFlush();
}