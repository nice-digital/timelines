using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace NICE.Timelines
{
    class Program
    {
        static void Main(string[] args)
        {
            //unusually for a console app, using appsettings.json + secrets.json for configuration, for consistency with other projects and also for the secrets support - because it's a public repo.
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
        }
    }
}
