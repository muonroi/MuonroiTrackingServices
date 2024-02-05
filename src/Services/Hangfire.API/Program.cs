using Common.Logging;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    Log.Information("Starting HangfireJob API up");

    builder.Host.UseSerilog(Serilogger.Configure);

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
    Log.Fatal(ex, "HangfireJob API terminated unexpectedly");
}
finally
{
    Log.Information("Shut down HangfireJob API complete");
    Log.CloseAndFlush();
}