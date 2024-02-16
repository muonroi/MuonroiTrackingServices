namespace Product.API.Extensions
{
    public static class ConfigureHostExtension
    {
        public static void AddConfigurationHost(this ConfigureHostBuilder configurationBuilder)
        {
            configurationBuilder.ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            });

        }
    }
}
