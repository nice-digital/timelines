using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NICE.Logging;
using NICE.Logging.Sinks.RabbitMQ;
using Serilog;
using Serilog.Events;

namespace NICE.Timelines
{
    public class SeriLogger
    {
        public static void Configure(IConfiguration configuration)
        {
            // read appsettings
            var logCfg = configuration.GetSection("Logging");

            //loggerFactory.AddConsole(logCfg); // add provider to send logs to System.Console.WriteLine()
            //loggerFactory.AddDebug(); // add provider to send logs to System.Diagnostics.Debug.WriteLine()

            var rabbitSettingsFound = int.TryParse(logCfg["RabbitMQPort"], out var rPort);
            bool.TryParse(logCfg["UseRabbit"], out var useRabbit);
            string logFilePath = logCfg["LogFilePath"];
            Enum.TryParse(logCfg["SerilogMinLevel"], out LogEventLevel serilogMinLevel);

            var formatter = new NiceSerilogFormatter(logCfg["Environment"], "Timelines");
            var logConfig = new LoggerConfiguration()
              .MinimumLevel.Is(serilogMinLevel);

            if (rabbitSettingsFound && useRabbit)
            {
                var rHost = logCfg["RabbitMQHost"];

                var rabbit = new RabbitMQConfiguration
                {
                    Hostname = rHost,
                    Port = rPort,
                    Protocol = RabbitMQ.Client.Protocols.AMQP_0_9_1,
                    Exchange = "logging.application.serilog",
                    ExchangeType = "topic"
                };

                logConfig.WriteTo.RabbitMQ(rabbit, formatter);
            }

            bool.TryParse(logCfg["UseFile"], out var useFile);

            if (useFile) //probably dev only
                logConfig.WriteTo.RollingFile(formatter, logFilePath, fileSizeLimitBytes: 5000000, retainedFileCountLimit: 5, flushToDiskInterval: TimeSpan.FromSeconds(20));

            Log.Logger = logConfig.CreateLogger();

            // add serilog provider (this is the hook)
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddSerilog();

            // clean up on shutdown
            //appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }
    }
}

