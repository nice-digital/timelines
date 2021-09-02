using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NICE.Timelines.Configuration;
using NICE.Timelines.DB.Models;
using NICE.Timelines.DB.Services;
using NICE.Timelines.Services;
using Serilog;

namespace NICE.Timelines
{
    class Program
    {
        private static ServiceProvider _serviceProvider;

        static async Task Main(string[] args)
        {
            //unusually for a console app, using appsettings.json + secrets.json for configuration, for consistency with other projects and also for the secrets support - because it's a public repo.
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            ClickUpConfig clickUpConfig = new ClickUpConfig();
            Configuration.Bind("ClickUp", clickUpConfig);
            EmailConfig emailConfig = new EmailConfig();
            Configuration.Bind("Email", emailConfig);

            RegisterServices(clickUpConfig, emailConfig, Configuration.GetConnectionString("DefaultConnection"));

            SeriLogger.Configure(Configuration);
 
            var scope = _serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                var context = scope.ServiceProvider.GetService<TimelinesContext>();
                context.Database.Migrate();
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while migrating the database.");
            }

            logger.LogInformation("Start");
            try
            {
                await scope.ServiceProvider.GetRequiredService<ISyncService>().Process(); //entry point
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred during the process.");
            }
     
            logger.LogInformation("End");

            DisposeServices();
        }

        private static void RegisterServices(ClickUpConfig clickUpConfig, EmailConfig emailConfig, string databaseConnectionString)
        {
            var services = new ServiceCollection(); //again unusually for a console app, setting up DI, in order to support testing.

            services.AddLogging(configure => configure.AddSerilog());
            services.AddSingleton(serviceProvider => clickUpConfig);
            services.AddSingleton(serviceProvider => emailConfig);
            services.AddTransient<ISyncService, SyncService>();
            services.AddTransient<IClickUpService, ClickUpService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddHttpClient();

            var contextOptionsBuilder = new DbContextOptionsBuilder<TimelinesContext>();
            services.TryAddSingleton<IDbContextOptionsBuilderInfrastructure>(contextOptionsBuilder);
            services.AddDbContext<TimelinesContext>(options => options.UseSqlServer(databaseConnectionString));

            services.AddTransient<IConversionService, ConversionService>();
            services.AddTransient<IDatabaseService, DatabaseService>();

            _serviceProvider = services.BuildServiceProvider(validateScopes: true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
                return;

            if (_serviceProvider is IDisposable)
                _serviceProvider.Dispose();
        }
    }
}
