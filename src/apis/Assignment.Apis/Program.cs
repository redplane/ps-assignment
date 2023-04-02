
using Microsoft.Extensions.Configuration.Json;

namespace Assignment.Apis
{
    public class Program
    {
        #region Methods

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureAppConfiguration(configurationBuilder =>
                        {
                            // Get the environment variable which is attached to application.
                            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                            var applicationConfigurationFile =
                                Environment.GetEnvironmentVariable("APPLICATION_CONFIGURATION_FILE");

                            var index = 0;
                            while (index < configurationBuilder.Sources.Count)
                            {
                                var configurationSource = configurationBuilder.Sources[index];
                                if (!(configurationSource is JsonConfigurationSource))
                                {
                                    index++;
                                    continue;
                                }

                                configurationBuilder.Sources.RemoveAt(index);
                            }

                            // Configuration build up.
                            configurationBuilder
                                .SetBasePath(Directory.GetCurrentDirectory());

                            // Add local appsettings.json file.
                            configurationBuilder.AddJsonFile("appsettings.json", false, true);

                            if ("Development".Equals(environment, StringComparison.InvariantCultureIgnoreCase))
                            {
                                configurationBuilder
                                    .AddJsonFile($"appsettings.{environment}.json", true, true)
                                    .AddEnvironmentVariables()
                                    .AddUserSecrets<Startup>(true, true);

                                if (File.Exists(applicationConfigurationFile))
                                    configurationBuilder.AddJsonFile(applicationConfigurationFile, true, true);
                            }
                            else if (!string.IsNullOrWhiteSpace(environment))
                            {
                                if (File.Exists(applicationConfigurationFile))
                                    configurationBuilder.AddJsonFile(applicationConfigurationFile, true, true);

                                configurationBuilder
                                    .AddJsonFile($"appsettings.{environment}.json", true, true)
                                    .AddEnvironmentVariables();
                            }
                            else
                            {
                                if (File.Exists(applicationConfigurationFile))
                                    configurationBuilder.AddJsonFile(applicationConfigurationFile, true, true);
                            }
                        })
                        .UseStartup<Startup>();
                });
        }

        #endregion
    }
}