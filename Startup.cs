using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(swaggerAzFunc1.Startup))]

namespace swaggerAzFunc1
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(path: "local.settings.json", optional: true)
                .Build();
            
            builder.Services.AddLogging(logging => {
                logging.AddConsole();
                logging.AddDebug();
            });

            builder.Services.AddSingleton(configuration);
            builder.Services.AddSingleton(new HttpClient() { BaseAddress = GetBaseAddress() });
            builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
            builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            builder.Services.AddSingleton(GetJsonSerializerOptions());
            builder.Services.AddSingleton<IValuesService, ValuesService>();
        }

        /// <summary>
        /// Gets the base address of this site.
        /// WEBSITE_HOSTNAME: running on Azure
        /// localhost: running locally (i.e. tests)
        /// </summary>
        public static Uri GetBaseAddress()
        {
            string website_hostname = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME") ?? "localhost";
            string baseAddress = $"https://{website_hostname}";
            if(string.IsNullOrWhiteSpace(baseAddress))
            {
                throw new ArgumentException(nameof(baseAddress));
            }
            return new Uri(baseAddress);
        }

        /// <summary>
        /// Gets a configured <c>JsonSerializerOptions</c>.
        /// </summary>
        public static JsonSerializerOptions GetJsonSerializerOptions()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }
    }
}